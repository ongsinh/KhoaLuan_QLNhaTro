using KhoaLuan_QLNhaTro.Models;
using KhoaLuan_QLNhaTro.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace KhoaLuan_QLNhaTro.Controllers
{
    public class RoomController : BaseController
    {
        //private readonly NhaTroDbContext _context;

        //public RoomController(NhaTroDbContext context)
        //{
        //    _context = context;
        //}

        public RoomController(NhaTroDbContext context) : base(context)
        {
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

            if (room == null)
            {
                return NotFound("Phòng không tồn tại.");
            }

            // Kiểm tra nếu phòng không có UserID
            if (room.UserId == null)
            {
                TempData["Message"] = "Phòng chưa có người ở."; // Truyền thông báo qua TempData
                return RedirectToAction("RoomMain"); // Chuyển hướng tới danh sách phòng (hoặc trang khác phù hợp)
            }

            // Truy vấn danh sách dịch vụ liên kết với phòng qua RoomService
            var services = _context.RoomsServices
                .Where(rs => rs.RoomId == id)
                .Include(rs => rs.Service) // Bao gồm thông tin của bảng Service
                .ToList();

            // Truyền dữ liệu phòng và dịch vụ sang view
            ViewBag.Services = services;
            return View(room);
        }

        //public IActionResult RoomMain()
        //{
        //    // Tải dữ liệu từ database vào bộ nhớ
        //    var rooms = _context.Rooms
        //        .Include(r => r.Contract)
        //        .Include(r => r.User)
        //        .ToList();

        //    // Thực hiện sắp xếp trong bộ nhớ
        //    var sortedRooms = rooms
        //        .OrderBy(r =>
        //        {
        //            var digits = new string(r.Name.Where(char.IsDigit).ToArray());
        //            return int.TryParse(digits, out int number) ? number : int.MaxValue;
        //        })
        //        .ThenBy(r => r.Name)
        //        .ToList();

        //    return View(sortedRooms);
        //}

        public IActionResult RoomMain(Guid? houseId)
        {
            // Kiểm tra nếu houseId không có trong URL
            if (houseId == null || houseId == Guid.Empty)
            {
                // Thử lấy houseId từ session
                var houseIdString = HttpContext.Session.GetString("HouseId");
                if (!string.IsNullOrEmpty(houseIdString) && Guid.TryParse(houseIdString, out Guid sessionHouseId))
                {
                    houseId = sessionHouseId;
                }
                else
                {
                    // Nếu không có houseId trong session, chuyển về trang mặc định
                    return RedirectToAction("Index", "Home");
                }
            }

            // Lấy thông tin nhà trọ từ cơ sở dữ liệu
            var house = _context.Houses.FirstOrDefault(h => h.Id == houseId.Value);
            if (house == null)
            {
                return NotFound(); // Nếu không tìm thấy nhà trọ
            }

            // Lấy danh sách phòng của nhà trọ
            var rooms = _context.Rooms
                .Include(r => r.Contract)  // Đảm bảo hợp đồng được nạp đầy đủ
                .Where(r => r.HouseId == houseId.Value)
                .OrderBy(r => r.FLoorNumber)
                .ThenBy(r => r.Name)
                .ToList();

            // Tạo ViewModel để truyền dữ liệu vào view
            var model = new RoomMainViewModel
            {
                HouseId = houseId.Value,
                Rooms = rooms
            };

            // Truyền tên nhà trọ vào ViewBag
            ViewBag.HouseName = house.Name;

            // Lưu houseId vào session để dùng cho các lần truy cập sau
            HttpContext.Session.SetString("HouseId", house.Id.ToString());
            HttpContext.Session.SetString("HouseName", house.Name);

            return View(model);
        }

        //public IActionResult RoomMain(Guid houseId)
        //{
        //    // Lấy thông tin nhà trọ từ cơ sở dữ liệu
        //    var house = _context.Houses.FirstOrDefault(h => h.Id == houseId);
        //    if (house == null)
        //    {
        //        return NotFound();
        //    }

        //    // Lấy danh sách phòng của nhà trọ
        //    var rooms = _context.Rooms
        //        .Where(r => r.HouseId == houseId)
        //        .OrderBy(r => r.FLoorNumber)
        //        .ThenBy(r => r.Name)
        //        .ToList();

        //    var model = new RoomMainViewModel
        //    {
        //        HouseId = houseId,
        //        Rooms = rooms
        //    };

        //    // Truyền tên nhà trọ vào ViewBag
        //    ViewBag.HouseName = house.Name;

        //    return View(model);
        //}



        //public IActionResult RoomMain(Guid houseId)
        //{
        //    var house = _context.Houses.FirstOrDefault(h => h.Id == houseId);
        //    if (house == null) return NotFound();

        //    var rooms = _context.Rooms
        //        .Where(r => r.HouseId == houseId)
        //        .OrderBy(r => r.FLoorNumber)
        //        .ThenBy(r => r.Name)
        //        .ToList();

        //    var model = new RoomMainViewModel
        //    {
        //        HouseId = houseId,
        //        Rooms = rooms
        //    };

        //    ViewBag.HouseName = house.Name;

        //    return View(model);
        //}



        //public IActionResult RoomMain(Guid houseId)
        //{
        //    var house = _context.Houses.FirstOrDefault(h => h.Id == houseId);
        //    if (house == null) return NotFound();

        //    var rooms = _context.Rooms
        //        .Where(r => r.HouseId == houseId)
        //        .OrderBy(r => r.FLoorNumber)
        //        .ThenBy(r => r.Name)
        //        .ToList();

        //    var model = new RoomMainViewModel
        //    {
        //        HouseId = houseId,
        //        Rooms = rooms
        //    };


        //    // Truyền houseId vào ViewBag
        //    //ViewBag.HouseId = houseId;

        //    //// Lấy tên nhà trọ từ session nếu cần
        //    //var houseName = HttpContext.Session.GetString("HouseName");
        //    //if (string.IsNullOrEmpty(houseName))
        //    //{
        //    //    houseName = "Chưa có nhà trọ";
        //    //}

        //    //// Truyền tên nhà trọ vào ViewBag để hiển thị
        //    //ViewBag.HouseName = houseName;
        //    return View(model);
        //}

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
                //room.FLoorNumber = model.FLoorNumber;
                room.Price = model.Price;
                room.Acreage = model.Acreage;
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return View("RoomMain");
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
