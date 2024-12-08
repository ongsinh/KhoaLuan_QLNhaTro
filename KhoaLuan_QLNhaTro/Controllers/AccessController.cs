
using KhoaLuan_QLNhaTro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace KhoaLuan_QLNhaTro.Controllers
{
    public class AccessController : BaseController
    {
        //private readonly NhaTroDbContext _dbContext;
        //public AccessController(NhaTroDbContext dbContext)
        //{
        //    _dbContext = dbContext;
        //}
        public AccessController(NhaTroDbContext context) : base(context)
        {
        }
        // GET: Đăng nhập
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string Phone, string Password)
        {
            if (string.IsNullOrEmpty(Phone) || string.IsNullOrEmpty(Password))
            {
                ViewBag.ErrorMessage = "Vui lòng nhập đầy đủ thông tin.";
                return View(); // Quay lại view login nếu thiếu thông tin.
            }

            // Kiểm tra tài khoản và mật khẩu
            //var account = _context.Accounts
            //    .FirstOrDefault(a => a.Phone == Phone && a.Password == Password);

            //if (account == null)
            //{
            //    ViewBag.ErrorMessage = "SĐT hoặc mật khẩu không đúng.";
            //    return View(); // Quay lại view login nếu thông tin sai.
            //}
            // Kiểm tra tài khoản và mật khẩu
            var account = _context.Accounts
                .Include(a => a.User) // Bao gồm thông tin liên kết đến User
                .FirstOrDefault(a => a.Phone == Phone && a.Password == Password);

            if (account == null)
            {
                ViewBag.ErrorMessage = "SĐT hoặc mật khẩu không đúng.";
                return View(); // Quay lại view login nếu thông tin sai.
            }

            HttpContext.Session.SetString("UserId", account.User.Id.ToString()); // Lưu UserId
            // Lưu thông tin người dùng vào session
            HttpContext.Session.SetString("UserPhone", account.Phone);
            HttpContext.Session.SetString("UserRole", account.RoleId);

            // Phân quyền theo RoleId
            if (account.RoleId == "Role_1") // Nếu là Admin
            {
                // Kiểm tra HouseId trong session
                var houseIdString = HttpContext.Session.GetString("HouseId");
                if (!string.IsNullOrEmpty(houseIdString) && Guid.TryParse(houseIdString, out Guid houseId))
                {
                    return RedirectToAction("RoomMain", "Room", new { houseId = houseId });
                }

                // Nếu không có HouseId, lấy nhà trọ mới nhất
                var latestHouse = _context.Houses
                    .OrderByDescending(h => h.CreateAt) // Lấy nhà trọ mới nhất
                    .FirstOrDefault();

                if (latestHouse != null)
                {
                    // Lưu thông tin nhà trọ mới nhất vào session
                    HttpContext.Session.SetString("HouseId", latestHouse.Id.ToString());
                    HttpContext.Session.SetString("HouseName", latestHouse.Name);
                    return RedirectToAction("RoomMain", "Room", new { houseId = latestHouse.Id });
                }

                // Nếu không có nhà trọ nào, chuyển đến trang mặc định
                return RedirectToAction("Index", "Home");
            }
            else if (account.RoleId == "Role_2") // Nếu là User
            {
                Console.WriteLine($"User {account.Phone} has Role_2");
                return RedirectToAction("TrangChuMain", "TrangChu");
            }
            else
            {
                Console.WriteLine($"Unknown RoleId: {account.RoleId}");
            }


            // Trường hợp không xác định vai trò
            //ViewBag.ErrorMessage = "Không xác định quyền truy cập.";
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Login(string Phone, string Password)
        //{
        //    if (string.IsNullOrEmpty(Phone) || string.IsNullOrEmpty(Password))
        //    {
        //        ViewBag.ErrorMessage = "Vui lòng nhập đầy đủ thông tin.";
        //        return View(); // Quay lại view login nếu thiếu thông tin.
        //    }

        //    // Kiểm tra tài khoản và mật khẩu
        //    var account = _context.Accounts
        //        .FirstOrDefault(a => a.Phone == Phone && a.Password == Password);

        //    if (account == null)
        //    {
        //        ViewBag.ErrorMessage = "SĐT hoặc mật khẩu không đúng.";
        //        return View(); // Quay lại view login nếu thông tin sai.
        //    }

        //    // Lưu thông tin người dùng vào session
        //    HttpContext.Session.SetString("UserPhone", account.Phone);
        //    HttpContext.Session.SetString("UserRole", account.RoleId);

        //    // Kiểm tra vai trò
        //    if (account.RoleId == "Role_1") // Nếu là Admin
        //    {
        //        var houseIdString = HttpContext.Session.GetString("HouseId");
        //        Guid houseId;

        //        if (!string.IsNullOrEmpty(houseIdString) && Guid.TryParse(houseIdString, out houseId))
        //        {
        //            // Chuyển hướng tới RoomMain với HouseId đã lưu trong session
        //            return RedirectToAction("RoomMain", "Room", new { houseId = houseId });
        //        }
        //    }
        //    else if (account.RoleId == "Role_2") // Nếu là User
        //    {
        //        // Chuyển hướng tới TrangChuMain hoặc trang người dùng
        //        return RedirectToAction("TrangChuMain", "TrangChu");
        //    }

        //    // Trường hợp không xác định vai trò
        //    ViewBag.ErrorMessage = "Không xác định quyền truy cập.";
        //    // Kiểm tra xem có HouseId trong session không
        //    //var houseIdString = HttpContext.Session.GetString("HouseId");
        //    //Guid houseId;

        //    //if (!string.IsNullOrEmpty(houseIdString) && Guid.TryParse(houseIdString, out houseId))
        //    //{
        //    //    // Chuyển hướng tới RoomMain với HouseId đã lưu trong session
        //    //    return RedirectToAction("RoomMain", "Room", new { houseId = houseId });
        //    //}

        //    // Nếu không có HouseId trong session, lấy nhà trọ mới nhất
        //    var latestHouse = _context.Houses
        //        .OrderByDescending(h => h.CreateAt) // Hoặc một trường khác để xác định nhà trọ "mới nhất"
        //        .FirstOrDefault();

        //    if (latestHouse != null)
        //    {
        //        // Lưu thông tin nhà trọ mới nhất vào session
        //        HttpContext.Session.SetString("HouseId", latestHouse.Id.ToString());
        //        HttpContext.Session.SetString("HouseName", latestHouse.Name);

        //        // Chuyển hướng tới RoomMain của nhà trọ mới nhất
        //        return RedirectToAction("RoomMain", "Room", new { houseId = latestHouse.Id });
        //    }

        //    // Nếu không có nhà trọ nào, chuyển hướng đến trang mặc định
        //    return RedirectToAction("Index", "Home");
        //}

        //// POST: Xử lý đăng nhập
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Login(string Phone, string Password)
        //{
        //    if (string.IsNullOrEmpty(Phone) || string.IsNullOrEmpty(Password))
        //    {
        //        ViewBag.ErrorMessage = "Vui lòng nhập đầy đủ thông tin.";
        //        return View();  // Return the login view again if validation fails.
        //    }

        //    // Kiểm tra tài khoản và mật khẩu
        //    var account = _context.Accounts
        //        .Where(a => a.Phone == Phone && a.Password == Password)
        //        .FirstOrDefault();

        //    if (account == null)
        //    {
        //        ViewBag.ErrorMessage = "SĐT hoặc mật khẩu không đúng.";
        //        return View();  // Return the login view again if login fails.
        //    }

        //    // Lưu thông tin người dùng vào session (hoặc cookie)
        //    HttpContext.Session.SetString("UserPhone", account.Phone);
        //    HttpContext.Session.SetString("UserRole", account.RoleId);

        //    // Redirect đến trang chủ sau khi đăng nhập thành công
        //    return RedirectToAction("AssetMain", "Asset");
        //    // Xử lý đăng nhập ở đây
        //    //var user = _dbContext.Users.FirstOrDefault(u => u.Account.Phone == Phone && u.Account.Password == Password);

        //    //if (user != null)
        //    //{
        //    //    // Lưu thông tin nhà trọ vào session (nếu có)
        //    //    var houseId = user.LastHouseId; // Lấy nhà trọ người dùng đã chọn trước đó
        //    //    var house = _dbContext.Houses.FirstOrDefault(h => h.Id == houseId);

        //    //    if (house != null)
        //    //    {
        //    //        // Lưu nhà trọ vào session
        //    //        HttpContext.Session.SetString("HouseId", house.Id.ToString());
        //    //        HttpContext.Session.SetString("HouseName", house.Name);
        //    //    }

        //    //    // Chuyển hướng đến trang chính hoặc trang nhà trọ
        //    //    return RedirectToAction("RoomMain", "Room");
        //    //}
        //    //else
        //    //{
        //    //    // Thông báo lỗi đăng nhập
        //    //    ViewBag.ErrorMessage = "Sai tên đăng nhập hoặc mật khẩu!";
        //    //    return View();
        //    //}
        //}


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
            var existingAccount = _context.Accounts
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
                _context.Accounts.Add(model);
                _context.Users.Add(model.User);
                _context.SaveChanges();  // Lưu dữ liệu vào DB
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
