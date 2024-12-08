using KhoaLuan_QLNhaTro.Models;
using KhoaLuan_QLNhaTro.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KhoaLuan_QLNhaTro.Controllers
{
    public class ServiceController : BaseController
    {
        //private readonly NhaTroDbContext _context;

        //public ServiceController (NhaTroDbContext context) 
        //{
        //    _context = context;
        //}

        public ServiceController(NhaTroDbContext context) : base(context)
        {
        }
        
        public IActionResult ServiceMain(Guid idHouse)
        {
            if (idHouse == Guid.Empty)
            {
                // Handle the case where idHouse is invalid
                return NotFound("ID House is invalid.");
            }

            // Your code to fetch services and rooms
            var serviceMainModels = _context.Services
                .Include(s => s.RoomServices)
                .ThenInclude(rs => rs.Room)
                .Where(s => s.RoomServices.Any(rs => rs.Room.HouseId == idHouse))
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

            ViewBag.IdHouse = idHouse; // Pass idHouse to ViewBag
            return View(serviceMainModels);
        }

        [HttpGet]
        public IActionResult AddService(Guid idHouse)
        {
            var rooms = _context.Rooms.Where(r => r.HouseId == idHouse).ToList();
            ViewBag.Rooms = rooms;
            ViewBag.IdHouse = idHouse; // Truyền idHouse xuống View
            return PartialView("AddService"); // Trả về một PartialView để hiển thị trong modal
        }

        [HttpPost]
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
                return RedirectToAction("ServiceMain", new { idHouse = model.IdHouse });
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
                // Find the service and its associated rooms
                var service = _context.Services
                    .Include(s => s.RoomServices)
                    .ThenInclude(rs => rs.Room)
                    .FirstOrDefault(s => s.Id == id);

                if (service == null)
                {
                    return NotFound("Dịch vụ không tồn tại.");
                }

                // Get HouseId from one of the service's rooms
                var houseId = service.RoomServices.FirstOrDefault()?.Room.HouseId;

                if (houseId == null || houseId == Guid.Empty)
                {
                    return NotFound("Không tìm thấy nhà trọ hợp lệ.");
                }

                // Fetch all rooms belonging to the house
                var allRooms = _context.Rooms
                    .Where(r => r.HouseId == houseId)
                    .ToList();

                // Create the ViewModel
                var serviceDetails = new ServiceViewModel
                {
                    Id = service.Id,
                    Name = service.Name,
                    Price = service.Price,
                    Unit = service.Unit,
                    SelectedRooms = service.RoomServices.Select(rs => rs.RoomId).ToList(),
                    AllRooms = allRooms, // Pass rooms belonging to the house
                    IdHouse = houseId.Value // Ensure IdHouse is set
                };

                return PartialView("EditService", serviceDetails);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi xảy ra: {ex.Message}");
                return StatusCode(500, "Có lỗi xảy ra khi xử lý yêu cầu.");
            }
        }

        // POST: Cập nhật thông tin dịch vụ
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditService(ServiceViewModel model)
        {
            // Loại bỏ AllRooms khỏi ModelState để tránh lỗi ràng buộc
            ModelState.Remove("AllRooms");

            if (!ModelState.IsValid)
            {
                return View(model); // Trả về lại form nếu model không hợp lệ
            }

            try
            {
                var service = _context.Services
                    .Include(s => s.RoomServices)
                    .FirstOrDefault(s => s.Id == model.Id);

                if (service == null)
                    return NotFound();

                // Cập nhật thông tin dịch vụ
                service.Name = model.Name;
                service.Price = model.Price;
                service.Unit = model.Unit;

                // Xóa các liên kết phòng cũ
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

                // Điều hướng về trang ServiceMain và giữ lại idHouse
                return RedirectToAction("ServiceMain", new { idHouse = model.IdHouse });
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
