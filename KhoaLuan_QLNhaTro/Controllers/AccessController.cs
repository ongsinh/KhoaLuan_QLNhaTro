
using KhoaLuan_QLNhaTro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace KhoaLuan_QLNhaTro.Controllers
{
    public class AccessController : Controller
    {
        private readonly NhaTroDbContext _dbContext;
        public AccessController(NhaTroDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        // GET: Đăng nhập
        public IActionResult Login()
        {
            return View();
        }

        // POST: Xử lý đăng nhập
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string Phone, string Password)
        {
            if (string.IsNullOrEmpty(Phone) || string.IsNullOrEmpty(Password))
            {
                ViewBag.ErrorMessage = "Vui lòng nhập đầy đủ thông tin.";
                return View();  // Return the login view again if validation fails.
            }

            // Kiểm tra tài khoản và mật khẩu
            var account = _dbContext.Accounts
                .Where(a => a.Phone == Phone && a.Password == Password)
                .FirstOrDefault();

            if (account == null)
            {
                ViewBag.ErrorMessage = "SĐT hoặc mật khẩu không đúng.";
                return View();  // Return the login view again if login fails.
            }

            // Lưu thông tin người dùng vào session (hoặc cookie)
            HttpContext.Session.SetString("UserPhone", account.Phone);
            HttpContext.Session.SetString("UserRole", account.RoleId);

            // Redirect đến trang chủ sau khi đăng nhập thành công
            return RedirectToAction("ServiceMain", "Service");
        }


        // Đăng xuất
        public IActionResult Logout()
        {
            // Xóa session khi người dùng đăng xuất
            HttpContext.Session.Remove("UserPhone");
            HttpContext.Session.Remove("UserRole");

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Account model)
        {
            // Kiểm tra dữ liệu đầu vào
            if (model.User == null || string.IsNullOrEmpty(model.User.Name) || string.IsNullOrEmpty(model.User.Email))
            {
                ViewBag.ErrorMessage = "Vui lòng nhập thông tin đầy đủ cho người dùng.";
                return View(model);
            }

            if (string.IsNullOrEmpty(model.Phone) || !Regex.IsMatch(model.Phone, @"^\d{10,12}$"))
            {
                ViewBag.ErrorMessage = "Số điện thoại không hợp lệ.";
                return View(model);
            }

            if (model.Password.Length < 8)
            {
                ViewBag.ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự.";
                return View(model);
            }

            // Kiểm tra email hoặc số điện thoại đã tồn tại
            var existingAccount = _dbContext.Accounts
                .Include(a => a.User)
                .FirstOrDefault(a => a.User.Email == model.User.Email || a.Phone == model.Phone);

            if (existingAccount != null)
            {
                ViewBag.ErrorMessage = "Email hoặc số điện thoại đã tồn tại.";
                return View(model);
            }

            // Cung cấp giá trị mặc định cho CCCD nếu không có
            if (string.IsNullOrEmpty(model.User.CCCD))
            {
                model.User.CCCD = "Chưa có CCCD"; // Cung cấp giá trị mặc định cho CCCD
            }

            // Cung cấp giá trị mặc định cho Gender nếu không có
            if (string.IsNullOrEmpty(model.User.Gender))
            {
                model.User.Gender = "Không xác định"; // Cung cấp giá trị mặc định cho Gender
            }

            // Cung cấp giá trị mặc định cho Address nếu không có
            if (model.User != null && string.IsNullOrEmpty(model.User.Address))
            {
                model.User.Address = "Chưa có địa chỉ"; // Cung cấp giá trị mặc định cho Address
            }

            // Thiết lập giá trị mặc định cho RoleId nếu không có
            if (string.IsNullOrEmpty(model.RoleId))
            {
                model.RoleId = "Role_1"; // Gán giá trị mặc định cho RoleId
            }

            // Gán GUID mới cho tài khoản và người dùng
            model.Id = Guid.NewGuid();  // Tạo GUID cho tài khoản
            model.User.Id = Guid.NewGuid();  // Tạo GUID cho người dùng

            try
            {
                // Thêm tài khoản và người dùng mới vào cơ sở dữ liệu
                _dbContext.Accounts.Add(model);
                _dbContext.Users.Add(model.User);
                _dbContext.SaveChanges();  // Lưu dữ liệu vào DB
            }
            catch (DbUpdateException ex)
            {
                // Log lỗi chi tiết để tìm hiểu nguyên nhân
                Console.WriteLine(ex.InnerException?.Message);
                ViewBag.ErrorMessage = "Đã xảy ra lỗi khi lưu dữ liệu. Vui lòng thử lại.";
                return View(model);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi chung
                Console.WriteLine(ex.Message);
                ViewBag.ErrorMessage = "Đã xảy ra lỗi không xác định. Vui lòng thử lại.";
                return View(model);
            }

            return RedirectToAction("Login", "Access");
        }
    }
}
