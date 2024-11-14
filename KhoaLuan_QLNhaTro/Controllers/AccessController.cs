using KhoaLuan_QLNhaTro.Models;
using Microsoft.AspNetCore.Mvc;

namespace KhoaLuan_QLNhaTro.Controllers
{
    public class AccessController : Controller
    {
        private readonly NhaTroDbContext _dbContext;
        public AccessController(NhaTroDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpPost]
        public IActionResult Login(Account model)
        {

            var account = _dbContext.Accounts.FirstOrDefault(u => u.User.Email == model.User.Email && u.Password == model.Password);
            if (account != null)
            {
                HttpContext.Session.SetString("Username", model.User.Email.ToString());
                HttpContext.Session.SetString("NameLogined", account.User.Name);
                HttpContext.Session.SetString("Role", account.RoleId);
                HttpContext.Session.SetString("UserId", account.Id.ToString());
                Console.WriteLine("Session Username: " + HttpContext.Session.GetString("Username"));
                Console.WriteLine("Name: " + account.User.Name);
                return RedirectToAction("RoomMain", "Room");
            }
            else
            {
                var error = "Tên đăng nhập hoặc mật khẩu không chính xác!";
                ViewBag.error = error;
            }
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Username") != null)
            {
                return RedirectToAction("RoomMain", "Room");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Logout()
        {
            //HttpContext.Session.Remove("UserName");
            //return RedirectToAction("Login");
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Access");
        }

        [HttpPost]
        public IActionResult Register(Account model)
        {
            bool emailExists = _dbContext.Accounts.Any(u => u.User.Email == model.User.Email);
            if (emailExists)
            {
                ModelState.AddModelError("Email", "Địa chỉ Email đã tồn tại");
                return View(model);
            }
            model.RoleId = "role2";
            _dbContext.Users.Add(model.User);
            _dbContext.SaveChanges();
            LoginAccount(model.User.Email, model.Password);

            return RedirectToAction("RoomMain", "Room");
        }

        private void LoginAccount(string email, string password)
        {
            var account = _dbContext.Accounts.FirstOrDefault(u => u.User.Email == email && u.Password == password);
            if (account != null)
            {
                HttpContext.Session.SetString("Username", email);
                HttpContext.Session.SetString("NameLogined", account.User.Name);
                HttpContext.Session.SetString("Role", account.RoleId);
                HttpContext.Session.SetString("UserId", account.Id.ToString());
                Console.WriteLine("Session Username: " + HttpContext.Session.GetString("Username"));
                Console.WriteLine("Name: " + account.User.Name);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
    }
}
