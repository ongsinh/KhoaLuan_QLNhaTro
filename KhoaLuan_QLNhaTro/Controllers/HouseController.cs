using KhoaLuan_QLNhaTro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace KhoaLuan_QLNhaTro.Controllers
{
    [Route("[controller]/[action]")]
    public class HouseController : BaseController
    {
        //private readonly NhaTroDbContext _context;

        //public HouseController(NhaTroDbContext context)
        //{
        //    _context = context;
        //}

        public HouseController(NhaTroDbContext context) : base(context)
        {
        }

        // Action để tạo nhà và các phòng
        public IActionResult CreateHouseAndRooms(string Name, string Address, int floorNumber, int[] RoomsPerFloor)
        {
            Console.WriteLine($"Name: {Name}, Address: {Address}, floorNumber: {floorNumber}, RoomsPerFloor: {string.Join(",", RoomsPerFloor)}");

            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Address) || RoomsPerFloor == null || RoomsPerFloor.Length == 0)
            {
                return Json(new { success = false, message = "Thông tin không hợp lệ" });
            }
            // Tạo mới nhà trọ
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

            // Tạo phòng trọ
            int roomNumber = 1;
            for (int i = 0; i < floorNumber; i++)
            {
                for (int j = 0; j < RoomsPerFloor[i]; j++)
                {
                    var room = new Room
                    {
                        Id = Guid.NewGuid(),
                        Name = $"Phòng {roomNumber++}",
                        HouseId = house.Id,
                        FLoorNumber = i + 1,
                        Price = 1000000,
                        Acreage = 30,
                        Status = "Còn trống",
                        //Acreage = 30,
                        CreateAt = DateTime.Now,
                        UpdateAt = DateTime.Now
                    };
                    _context.Rooms.Add(room);
                }
            }

            // Lưu thông tin nhà trọ vào Session
            HttpContext.Session.SetString("HouseId", house.Id.ToString());
            HttpContext.Session.SetString("HouseName", house.Name);  // Lưu cả tên nhà trọ

            try
            {
                _context.SaveChanges();
                return Json(new { success = true, houseId = house.Id, houseName = house.Name });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        public IActionResult HouseList()
        {
            // Lấy tất cả nhà trọ từ cơ sở dữ liệu
            var houses = _context.Houses.Select(h => new
            {
                id = h.Id,
                name = h.Name,
                address = h.Address
            }).ToList();

            return Json(houses); // Trả về danh sách nhà trọ dưới dạng JSON
        }

        // API để lưu HouseId vào Session
        [HttpPost]
        public IActionResult SetHouseSession(Guid houseId)
        {
            Console.WriteLine($"houseId nhận được: {houseId}");  // Kiểm tra giá trị houseId

            var house = _context.Houses.FirstOrDefault(h => h.Id == houseId);
            if (house != null)
            {
                // Lưu HouseId và HouseName vào session
                HttpContext.Session.SetString("HouseId", houseId.ToString());
                HttpContext.Session.SetString("HouseName", house.Name);

                // Debug thông qua Logger
                Console.WriteLine($"HouseId: {house.Id}, HouseName: {house.Name}");

                return Json(new { success = true });
            }

            // Nếu không tìm thấy nhà trọ, ghi log và trả về thông báo lỗi
            Console.WriteLine("Không tìm thấy nhà trọ với houseId: " + houseId);
            return Json(new { success = false, message = "Không tìm thấy nhà trọ." });
        }


        public IActionResult SelectHouse(Guid houseId)
        {
            var house = _context.Houses.FirstOrDefault(h => h.Id == houseId);
            if (house != null)
            {
                // Lưu thông tin nhà trọ vào session
                HttpContext.Session.SetString("HouseId", house.Id.ToString());
                HttpContext.Session.SetString("HouseName", house.Name);
            }

            return RedirectToAction("Room", "RoomMain", new { houseId = house.Id }); // Redirect to home page after selecting a house
        }



        [HttpPost]
        public IActionResult DeleteHouse(Guid houseId)
        {
            var house = _context.Houses.FirstOrDefault(h => h.Id == houseId);
            if (house == null)
            {
                return Json(new { success = false, message = "Không tìm thấy nhà trọ." });
            }

            // Xóa nhà trọ và các phòng liên quan
            var rooms = _context.Rooms.Where(r => r.HouseId == houseId).ToList();
            _context.Rooms.RemoveRange(rooms);
            _context.Houses.Remove(house);

            try
            {
                _context.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi khi xóa nhà trọ: " + ex.Message });
            }
        }


        [HttpGet]
        public IActionResult GetHouseById(Guid id)
        {
            var house = _context.Houses.Find(id);
            if (house != null)
            {
                return Json(new { success = true, house = house });
            }
            return Json(new { success = false });
        }
    }
}