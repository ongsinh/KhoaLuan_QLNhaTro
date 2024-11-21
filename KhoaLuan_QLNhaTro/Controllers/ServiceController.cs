using KhoaLuan_QLNhaTro.Models;
using KhoaLuan_QLNhaTro.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KhoaLuan_QLNhaTro.Controllers
{
    public class ServiceController : Controller
    {
        private readonly NhaTroDbContext _context;

        public ServiceController (NhaTroDbContext context) 
        {
            _context = context;
        }
        public IActionResult ServiceMain()
        {
            var serviceMainModels = _context.Services
                .Include(s => s.RoomServices) // Include liên kết RoomServices
                    .ThenInclude(rs => rs.Room) // Liên kết với Room
                .Select(service => new ServiceMainModel
                {
                    RoomService = new RoomServiceViewModel
                    {
                        Service = service,
                        Rooms = service.RoomServices.Select(rs => rs.Room).ToList()
                    },
                    DetailBillList = service.DetailBills.ToList()

                })
                .ToList();

            // Truyền danh sách phòng (nếu cần cho modal)
            ViewBag.Rooms = _context.Rooms.ToList();
            return View(serviceMainModels);
        }

        [HttpGet]
        public IActionResult AddService()
        {
            // Lấy danh sách các phòng
            ViewBag.Rooms = _context.Rooms.ToList();
            return View();
        }

        public IActionResult AddService(ServiceViewModel model)
        {
            try
            {
                var newService = new Service
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    Price = model.Price,
                    Unit = model.Unit,
                    CreateAt = DateTime.Now,
                    UpdateAt = DateTime.Now
                };

                _context.Services.Add(newService);

                if (model.SelectedRooms != null && model.SelectedRooms.Any())
                {
                    foreach (var roomId in model.SelectedRooms)
                    {
                        var roomService = new RoomService
                        {
                            RoomId = roomId,
                            ServiceId = newService.Id,
                            Number = model.Unit == "Tháng" ? 1 : 0,
                            Price = model.Unit == "Tháng" ? model.Price : 0
                        };

                        _context.RoomsServices.Add(roomService);
                    }
                }

                _context.SaveChanges();
                return RedirectToAction("ServiceMain");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi thêm dịch vụ: {ex.Message}");
                return StatusCode(500, "Có lỗi xảy ra trong quá trình xử lý.");
            }
        }

        [HttpGet]
        public IActionResult EditService(Guid id)
        {
            try
            {
                var service = _context.Services
                    .Include(s => s.RoomServices)
                    .ThenInclude(rs => rs.Room)
                    .FirstOrDefault(s => s.Id == id);

                if (service == null)
                    return NotFound();

                // Lấy danh sách tất cả các phòng
                var allRooms = _context.Rooms.ToList();

                // Tạo ViewModel
                var serviceDetails = new ServiceViewModel
                {
                    Id = service.Id,
                    Name = service.Name,
                    Price = service.Price,
                    Unit = service.Unit,
                    SelectedRooms = service.RoomServices.Select(rs => rs.RoomId).ToList(),
                    AllRooms = allRooms // Gửi tất cả các phòng xuống View
                };

                return PartialView("EditService", serviceDetails);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi xảy ra: " + ex.Message);
                return StatusCode(500, "Có lỗi xảy ra trong quá trình xử lý yêu cầu.");
            }
        }

        // POST: Cập nhật thông tin dịch vụ
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditService(ServiceViewModel model)
        {
            // Loại bỏ AllRooms khỏi ModelState để tránh lỗi ràng buộc
            ModelState.Remove("AllRooms");

            // Kiểm tra tính hợp lệ của model
            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState không hợp lệ");
                foreach (var key in ModelState.Keys)
                {
                    Console.WriteLine($"Key: {key}, Error: {string.Join(", ", ModelState[key].Errors.Select(e => e.ErrorMessage))}");
                }
                return View(model); // Trả về lại form nếu model không hợp lệ
            }

            try
            {
                var service = _context.Services
                    .Include(s => s.RoomServices)
                    .FirstOrDefault(s => s.Id == model.Id);

                if (service == null)
                    return NotFound();
                // Kiểm tra giá trị Price từ model
                Console.WriteLine("Price from form: " + model.Price); // Debug xem giá trị Price có hợp lệ không
                // Cập nhật thông tin dịch vụ
                service.Name = model.Name;
                service.Price = model.Price;
                service.Unit = model.Unit;

                // Xóa liên kết phòng cũ
                _context.RoomsServices.RemoveRange(service.RoomServices);

                // Thêm các liên kết phòng mới
                if (model.SelectedRooms != null && model.SelectedRooms.Any())
                {
                    var roomServices = model.SelectedRooms
                        .Select(roomId => new RoomService
                        {
                            RoomId = roomId,
                            ServiceId = service.Id
                        }).ToList();

                    service.RoomServices = roomServices;
                }

                _context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu

                // Chuyển hướng về trang ServiceMain sau khi cập nhật thành công
                return RedirectToAction("ServiceMain", "Service");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi xảy ra: " + ex.Message);
                return StatusCode(500, "Có lỗi xảy ra trong quá trình xử lý yêu cầu.");
            }
        }

        [HttpDelete]
        public IActionResult DeleteService(Guid id)
        {
            var service = _context.Services.Find(id);
            if (service == null)
            {
                return NotFound(); // Dịch vụ không tồn tại
            }

            _context.Services.Remove(service);
            _context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu

            return Ok(); // Trả về trạng thái thành công
        }
    }
}
