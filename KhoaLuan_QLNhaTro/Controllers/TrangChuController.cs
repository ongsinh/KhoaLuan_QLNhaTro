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

            // Lấy thông tin phòng của sinh viên
            var room = await _context.Rooms
                .Include(r => r.House) // Nếu cần thông tin nhà
                .FirstOrDefaultAsync(r => r.UserId == userGuid);

            // Lấy danh sách dịch vụ của phòng
            var services = room != null
                ? await _context.RoomsServices
                    .Include(rs => rs.Service) // Load thông tin chi tiết dịch vụ
                    .Where(rs => rs.RoomId == room.Id)
                    .ToListAsync()
                : new List<RoomService>();

            // Truyền dữ liệu sang ViewModel
            var model = new UserRoomViewModel
            {
                User = user,
                Room = room,
                Services = services
            };

            return View(model);
        }

    }
}
