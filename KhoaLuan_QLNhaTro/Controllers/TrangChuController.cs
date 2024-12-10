using KhoaLuan_QLNhaTro.Models;
using KhoaLuan_QLNhaTro.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KhoaLuan_QLNhaTro.Controllers
{
    public class TrangChuController : BaseController
    {
        public TrangChuController(NhaTroDbContext context) : base(context)
        {
        }
        public async Task<IActionResult> TrangChuMain()
        {
            // Lấy UserId từ Session hoặc Claims
            var userId = HttpContext.Session.GetString("UserId"); // Thay thế bằng cách lấy từ Token hoặc Claims nếu cần
            if (string.IsNullOrEmpty(userId))
            {
                Console.WriteLine("Session UserId is null or empty");
                return RedirectToAction("Login", "Access");
            }

            if (!Guid.TryParse(userId, out Guid userGuid))
            {
                Console.WriteLine("Invalid UserId format in session");
                return RedirectToAction("Login", "Access");
            }

            // Lấy thông tin sinh viên
            var user = await _context.Users
                .Include(u => u.Account)
                .FirstOrDefaultAsync(u => u.Id == userGuid);

            if (user == null) return NotFound("Không tìm thấy thông tin sinh viên.");
            var contract = await _context.Contracts
                .Include(c => c.Room)
                .ThenInclude(r => r.RoomServices)
                .ThenInclude(rs => rs.Service) // Bao gồm thông tin chi tiết của dịch vụ
                .FirstOrDefaultAsync(c => c.UserId == userGuid);

            if (contract == null || contract.Room == null)
            {
                Console.WriteLine("Contract or Room not found");
                return View(new UserRoomViewModel
                {
                    User = user,
                    Room = null,
                    Services = new List<RoomService>() // Không có dịch vụ
                });
            }

            // Lấy thông tin phòng và danh sách dịch vụ liên quan
            var room = contract.Room;
            var services = room.RoomServices.Select(rs => rs.Service).ToList(); // Lấy danh sách Service từ RoomServices

            // Chuẩn bị dữ liệu ViewModel
            var model = new UserRoomViewModel
            {
                User = user,
                Room = room,
                Services = room.RoomServices.ToList() // Lấy toàn bộ RoomService, vì View có thể cần thêm thông tin
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateInfo([FromForm] UserUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });
            }

            try
            {
                // Lấy người dùng từ database
             
                var user = _context.Users
                    .Include(u => u.Account) // Bao gồm liên kết đến bảng Account
                    .FirstOrDefault(u => u.Id == model.Id);

                if (user == null)
                {
                    return Json(new { success = false, message = "Người dùng không tồn tại!" });
                }

                // Cập nhật thông tin
                user.Name = model.Name;
                user.CCCD = model.CCCD;
                user.Gender = model.Gender;
                user.Email = model.Email;
                user.Dob = model.Dob;
                user.Address = model.Address;
                user.Account.Phone = model.Phone;
                user.Account.Password = model.Password; // Đảm bảo bảo mật nếu mật khẩu thay đổi.

                _context.SaveChanges(); // Lưu thay đổi vào DB.

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}
