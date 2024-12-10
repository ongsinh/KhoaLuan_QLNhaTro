using KhoaLuan_QLNhaTro.Models;
using KhoaLuan_QLNhaTro.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace KhoaLuan_QLNhaTro.Controllers
{
    public class HistoryController : Controller
    {
        private NhaTroDbContext _context;

        public HistoryController(NhaTroDbContext context)
        {
            _context = context;
        }
        public IActionResult HistoryMain()
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account"); // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
            }

            var userGuid = Guid.Parse(userId);
            var userBills = _context.Bills
                .Where(b => b.UserId == userGuid)
                .ToList();

            var billIds = userBills.Select(b => b.Id).ToList();

            var history = _context.PaymentResponseModels
                .Where(p => billIds.Contains(p.BillId))
                .Select(p => new HistoryViewModel
                {
                    billID = p.BillId,
                    TransactionId = p.TransactionId,
                    OrderDescription = p.OrderDescription,
                    Total = p.Total,
                    PaymentMethod = p.PaymentMethod,
                    Success = p.Success,
                    PaymentDate = p.PaymentDate
                })
                .OrderByDescending(p => p.PaymentDate)
                .ToList();
            return View(history);
        }

        [HttpGet]
        public IActionResult HistoryDetail(string billId)
        {
            if (string.IsNullOrEmpty(billId))
            {
                return Json(new { message = "BillId không hợp lệ." });
            }
            var roomName = _context.Bills
                           .Where(b => b.Id == billId)
                           .Select(b => b.Room.Name)  // Lấy tên phòng từ mối quan hệ với Room
                           .FirstOrDefault();
            var roomPrice = _context.Bills
                           .Where(b => b.Id == billId)
                           .Select(b => b.Room.Price)  // Lấy tên phòng từ mối quan hệ với Room
                           .FirstOrDefault();
            var creationDate = _context.Bills
                               .Where(b => b.Id == billId)
                               .Select(b => b.CreateAt)
                               .FirstOrDefault();

            var details = _context.DetailBills
                                  .Where(d => d.BillId == billId)
                                  .Select(d => new DetailHoaDonViewModel
                                  {
                                      ServiceName = d.Service.Name,
                                      Number = d.Number,
                                      OldNumber = d.OldNumber,
                                      NewNumber = d.NewNumber,
                                      Price = d.Price,
                                      Total = CalculateTotal(d)  // Tính tổng tiền cho dịch vụ
                                  })
                                  .ToList();

            if (details == null || !details.Any())
            {
                return Json(new { message = "Không có chi tiết hóa đơn." });
            }

            decimal totalAmount = details.Sum(d => d.Total);
            totalAmount += roomPrice;



            var history = _context.PaymentResponseModels
            .Where(p => p.BillId == billId)  // Lọc theo BillId cụ thể
            .Select(p => new HistoryDetailViewModel
            {
                TransactionId = p.TransactionId,
                OrderDescription = p.OrderDescription,
                Total = p.Total,
                PaymentMethod = p.PaymentMethod,
                Success = p.Success,
                PaymentDate = p.PaymentDate
            })
            .FirstOrDefault();
            // Trả về View với ViewModel
            var model = new HistoryDetailViewModel
            {
                Details = details,
                TotalAmount = totalAmount,
                billID = billId,
                NameRoom = roomName,
                roomPrice = roomPrice,
                CreateAt = creationDate,
                TransactionId = history.TransactionId,
                OrderDescription = history.OrderDescription,
                Total = history.Total,
                PaymentMethod = history.PaymentMethod,
                Success = history.Success,
                PaymentDate = history.PaymentDate

            };

            return View(model);
        }


        // Hàm tính tổng tiền cho dịch vụ (có thể có số lượng hoặc số mới, số cũ)
        public static decimal CalculateTotal(DetailBill detail)
        {
            decimal total = 0;

            // Kiểm tra và tính toán cho số lượng
            if (detail.Number.HasValue && detail.Number.Value > 0)
            {
                total = detail.Number.Value * detail.Price;
            }
            // Kiểm tra và tính toán cho sự thay đổi giữa số cũ và số mới
            else if (detail.NewNumber.HasValue && detail.OldNumber.HasValue)
            {
                total = (detail.NewNumber.Value - detail.OldNumber.Value) * detail.Price;
            }
            // Nếu không có dữ liệu hợp lệ, trả về 0
            return total;
        }
    }
}
