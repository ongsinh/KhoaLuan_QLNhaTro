using KhoaLuan_QLNhaTro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KhoaLuan_QLNhaTro.Controllers
{
    public class AdminController : Controller
    {
        public readonly NhaTroDbContext _context;
        public AdminController (NhaTroDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Main()
        {
            return View("Room/RoomMain");
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
            return View();
        }

        [Route("DetailRoom")]
        public IActionResult DetailRoom()
        {
            return View();
        }

        [Route("AssetMain")]
        public IActionResult AssetMain()
        {
            return View();
        }

        

        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }


        [Route("ContractMain")]
        public IActionResult ContractMain()
        {
            return View();
        }
    }
}
