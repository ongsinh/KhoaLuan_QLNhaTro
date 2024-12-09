using KhoaLuan_QLNhaTro.Models;
using KhoaLuan_QLNhaTro.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VnPayIntegration.Models;

namespace KhoaLuan_QLNhaTro.Controllers
{
    public class HoaDonController : Controller
    {
        private NhaTroDbContext _context;
        private readonly IVnPayService _vnPayService;

        public HoaDonController(NhaTroDbContext context, IVnPayService vnPayService)
        {
            _context = context;
            _vnPayService = vnPayService;
        }
        public IActionResult HoaDonMain()
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account"); // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
            }

            var userGuid = Guid.Parse(userId);

            // Tìm tất cả phòng theo UserId
            var rooms = _context.Rooms
                                .Where(r => r.UserId == userGuid);// Lọc các phòng theo UserId
            if (rooms == null || !rooms.Any())
            {
                // Nếu không có phòng, trả về view với thông báo
                ViewBag.Message = "Không có phòng nào được liên kết với tài khoản này.";
                return View(new List<Bill>()); // Trả về danh sách hóa đơn rỗng
            }

            // Tìm tất cả hóa đơn liên quan đến các phòng này
            var bills = _context.Bills
                                .Where(b => rooms.Select(r => r.Id).Contains(b.RoomId)) // Lấy hóa đơn theo RoomId
                                .OrderByDescending(b => b.CreateAt) // Sắp xếp theo ngày tạo (mới nhất lên trên)
                                .ToList();

            return View(bills); // Truyền danh sách hóa đơn vào View
        }

        [HttpGet]
        public IActionResult ThanhToan(string billId)
        {
            if (string.IsNullOrEmpty(billId))
            {
                return Json(new { message = "BillId không hợp lệ." });
            }
            var roomName = _context.Bills
                           .Where(b => b.Id == billId)
                           .Select(b => b.Room.Name)  // Lấy tên phòng từ mối quan hệ với Room
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

            // Trả về View với ViewModel
            var model = new HoaDonViewModel
            {
                Details = details,
                TotalAmount = totalAmount,
                billID = billId,
                NameRoom = roomName,
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

        [HttpPost]
        public IActionResult CreatePaymentUrlVnpay([FromBody] PaymentInformationModel model)
        {
            try
            {
                var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

                // Trả về URL thanh toán để frontend xử lý
                return Json(new { success = true, paymentUrl = url });
            }
            catch (Exception ex)
            {
                // Trả về lỗi nếu có
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult PaymentCallbackVnpay()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);
            var billid = response.BillId;
            var total = response.Total;
            if (response.Success)
            {
                var bill = _context.Bills.FirstOrDefault(b => b.Id == billid);
                if (bill != null)
                {
                    bill.Status = "Đã thanh toán";
                    bill.Total = total;

                    // Lưu thay đổi vào cơ sở dữ liệu
                    _context.SaveChanges();
                }
            }
            return View(response); // Trả về view với model PaymentResponseModel
        }
    }
}
