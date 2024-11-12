using Microsoft.AspNetCore.Mvc;

namespace KhoaLuan_QLNhaTro.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [Route("RoomMain")]
        public IActionResult Room()
        {
            return View("Room/RoomMain");
        }

        [Route("BillMain")]
        public IActionResult Bill()
        {
            return View("Bill/BillMain"); 
        }

        [Route("ReportMain")]
        public IActionResult Report()
        {
            return View("Report/ReportMain");
        }

        [Route("DetailRoom")]
        public IActionResult DetailRoom()
        {
            return View("Room/DetailRoom");
        }

    }
}
