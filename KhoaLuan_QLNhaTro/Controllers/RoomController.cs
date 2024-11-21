using KhoaLuan_QLNhaTro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace KhoaLuan_QLNhaTro.Controllers
{
    public class RoomController : Controller
    {
        private readonly NhaTroDbContext _context;

        public RoomController(NhaTroDbContext context)
        {
            _context = context;
        }

        public IActionResult AddHouse()
        {
            return View();
        }

        public IActionResult DetailRoom(Guid id)
        {
            var room = _context.Rooms
                .Include(r => r.User)
                .Include(r => r.Contract)
                .FirstOrDefault(r => r.Id == id);

            // Truy vấn danh sách dịch vụ liên kết với phòng qua RoomService
            var services = _context.RoomsServices
                .Where(rs => rs.RoomId == id)
                .Include(rs => rs.Service) // Bao gồm thông tin của bảng Service
                .ToList();

            // Truyền dữ liệu phòng và dịch vụ sang view
            ViewBag.Services = services;
            return View(room);
        }

        public IActionResult RoomMain()
        {
            // Tải dữ liệu từ database vào bộ nhớ
            var rooms = _context.Rooms
                .Include(r => r.Contract)
                .ToList();

            // Thực hiện sắp xếp trong bộ nhớ
            var sortedRooms = rooms
                .OrderBy(r =>
                {
                    var digits = new string(r.Name.Where(char.IsDigit).ToArray());
                    return int.TryParse(digits, out int number) ? number : int.MaxValue;
                })
                .ThenBy(r => r.Name)
                .ToList();

            return View(sortedRooms);
        }


        [HttpPost]
        public IActionResult AddRoom(Room room)
        {
            if (ModelState.IsValid)
            {
                // Thêm phòng trọ vào cơ sở dữ liệu
                _context.Rooms.Add(room);
                _context.SaveChanges();

                // Trả về thành công hoặc thông báo lỗi
                return Json(new { success = true, message = "Phòng trọ được thêm thành công." });
            }
            return Json(new { success = false, message = "Dữ liệu không hợp lệ." });
        }

        [HttpPost]
        [Route("/Room/CreateHouse")]
        public IActionResult CreateHouse(string Name, string Address, int floorNumber, List<int> floorRooms)
        {
            if (ModelState.IsValid)
            {
                var house = new House
                {
                    Id = Guid.NewGuid(),
                    Name = Name,
                    Address = Address,
                    floorNumber = floorNumber,
                    CreateAt = DateTime.Now,
                    UpdateAt = DateTime.Now
                };

                _context.Houses.Add(house);
                _context.SaveChanges();

                int roomCount = 1;
                for (int i = 0; i < floorRooms.Count; i++)
                {
                    int roomsInFloor = floorRooms[i]; // Số phòng ở tầng i
                    for (int j = 0; j < roomsInFloor; j++)
                    {
                        var room = new Room
                        {
                            Id = Guid.NewGuid(),
                            Name = $"Tầng {i + 1} - Phòng {roomCount++}",
                            FLoorNumber = i + 1,
                            HouseId = house.Id,
                            CreateAt = DateTime.Now,
                            UpdateAt = DateTime.Now
                        };

                        _context.Rooms.Add(room);
                    }
                }

                _context.SaveChanges();
                return RedirectToAction("RoomMain");
            }

            return View();
        }

        [HttpGet]
        public IActionResult GetRoomData(Guid id)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.Id == id); 
            if (room == null)
            {
                return Json(new { success = false, message = "Không tìm thấy phòng" });
            }

            return Json(new
            {
                success = true,
                name = room.Name,
                floorNumber = room.FLoorNumber,
                price = room.Price,
                acreage = room.Acreage
            });
        }

        [HttpPost]
        public ActionResult EditRoom(Room model)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.Id == model.Id);
            
            if (room != null)
            {
                room.Name = model.Name;
                room.FLoorNumber = model.FLoorNumber;
                room.Price = model.Price;
                room.Acreage = model.Acreage;
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public IActionResult GetByName(string Name)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return View("RoomMain", _context.Rooms.ToList());
            }

            // Sử dụng ToLower() để so sánh không phân biệt hoa/thường
            var rooms = _context.Rooms
                .Include(r => r.Contract)
                .Where(r => r.Name != null && r.Name.ToLower().Contains(Name.Trim().ToLower()))
                .ToList();

            return View("RoomMain", rooms);
        }


    }
}
