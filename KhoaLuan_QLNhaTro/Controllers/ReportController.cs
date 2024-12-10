using KhoaLuan_QLNhaTro.Models;
using Microsoft.AspNetCore.Mvc;

namespace KhoaLuan_QLNhaTro.Controllers
{
    public class ReportController : BaseController
    {
        public ReportController(NhaTroDbContext context) : base(context)
        {
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ReportMain(Guid idHouse)
        {
            
            // Truy vấn hóa đơn đã thanh toán theo HouseId
            var paidBills = _context.Bills
                .Where(b => b.Room.HouseId == idHouse && b.Status == "Đã thanh toán") // Lọc theo HouseId và trạng thái "Paid"
                .Select(b => new
                {
                    RoomName = b.Room.Name,
                    CreateAt = b.CreateAt,
                    Total = b.Total,
                })
                .ToList();

            // Tính tổng tiền của tất cả hóa đơn đã thanh toán
            var totalAmount = paidBills.Sum(b => b.Total);

            // Truyền danh sách hóa đơn và tổng tiền vào View
            ViewBag.PaidBills = paidBills;
            ViewBag.TotalAmount = totalAmount;
            ViewBag.idHouse = idHouse;
            return View();
        }

        public IActionResult FilterByMonthAndYear(Guid idHouse, int month, int year)
        {
            var paidBills = _context.Bills
                .Where(b => b.Room.HouseId == idHouse && b.Status == "Đã thanh toán"
                            && b.CreateAt.Month == month
                            && b.CreateAt.Year == year)
                .Select(b => new
                {
                    RoomName = b.Room.Name,
                    CreateAt = b.CreateAt,
                    Total = b.Total,
                })
                .ToList();

            var totalAmount = paidBills.Sum(b => b.Total);

            ViewBag.PaidBills = paidBills;
            ViewBag.TotalAmount = totalAmount;
            ViewBag.SelectedMonth = month;
            ViewBag.SelectedYear = year;

            return View("ReportMain");  // Trả về cùng một view, chỉ lọc theo tháng và năm
        }
    }
}
