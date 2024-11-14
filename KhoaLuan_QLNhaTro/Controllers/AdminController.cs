using Microsoft.AspNetCore.Mvc;

namespace KhoaLuan_QLNhaTro.Controllers
{
    public class AdminController : Controller
    {
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
            return View("Report/ReportMain");
        }

        [Route("DetailRoom")]
        public IActionResult DetailRoom()
        {
            return View("Room/DetailRoom");
        }

        [Route("AssetMain")]
        public IActionResult AssetMain()
        {
            return View("Asset/AssetMain");
        }

        [Route("ServiceMain")]
        public IActionResult ServiceMain()
        {
            return View("Service/ServiceMain");
        }

        [Route("Register")]
        public IActionResult Register()
        {
            return View("Access/Register");
        }

        [Route("Login")]
        public IActionResult Login()
        {
            return View("Access/Login");
        }

        [Route("AccountMain")]
        public IActionResult AccountMain()
        {
            return View("Account/AccountMain");
        }

        [Route("ContractMain")]
        public IActionResult ContractMain()
        {
            return View("Contract/ContractMain");
        }
    }
}
