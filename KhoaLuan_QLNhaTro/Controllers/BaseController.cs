using KhoaLuan_QLNhaTro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace KhoaLuan_QLNhaTro.Controllers
{
    public class BaseController : Controller
    {
        public readonly NhaTroDbContext _context;

        public BaseController(NhaTroDbContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var houseIdString = HttpContext.Session.GetString("HouseId");
            if (!string.IsNullOrEmpty(houseIdString))
            {
                ViewBag.HouseId = Guid.Parse(houseIdString);
            }
            var houseName = HttpContext.Session.GetString("HouseName");
            if (string.IsNullOrEmpty(houseName))
            {
                houseName = "Chưa có nhà trọ";
            }
            var houses = _context.Houses.ToList(); // Lấy tất cả nhà trọ từ database
            ViewBag.Houses = houses;

            // Truyền tên nhà trọ vào ViewBag để hiển thị
            ViewBag.HouseName = houseName;

            base.OnActionExecuting(context);
        }
    }
}
