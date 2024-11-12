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
