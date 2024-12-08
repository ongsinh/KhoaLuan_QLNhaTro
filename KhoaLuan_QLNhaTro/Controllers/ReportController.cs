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

        public IActionResult ReportMain()
        {
            return View();
        }
    }
}
