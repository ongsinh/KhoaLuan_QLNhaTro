using Microsoft.AspNetCore.Mvc;

namespace KhoaLuan_QLNhaTro.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [Route("Room")]
        public IActionResult Room()
        {
            return View();
        }

        [Route("Bill")]
        public IActionResult Bill()
        {
            return View(); 
        }

        [Route("Report")]
        public IActionResult Report()
        {
            return View();
        }

        [Route("DetailRoom")]
        public IActionResult DetailRoom()
        {
            return View();
        }

    }
}
