using KhoaLuan_QLNhaTro.Models;
using KhoaLuan_QLNhaTro.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KhoaLuan_QLNhaTro.Controllers
{
    public class AssetController : BaseController
    {
        //private readonly NhaTroDbContext _context;

        //public AssetController(NhaTroDbContext context)
        //{
        //    _context = context;
        //}

        public AssetController(NhaTroDbContext context) : base(context)
        {
        }
        
        public IActionResult AssetMain(Guid idHouse)
        {
            try
            {
                // Kiểm tra idHouse hợp lệ
                if (idHouse == Guid.Empty)
                {
                    return NotFound("Không tìm thấy nhà trọ hợp lệ.");
                }

                // Lấy danh sách tài sản theo HouseId
                var assets = _context.Assets
                    .Where(a => a.Room.HouseId == idHouse)
                    .Include(a => a.Room)
                    .ToList();

                ViewBag.IdHouse = idHouse; // Pass idHouse to ViewBag
                // Truyền dữ liệu xuống View
                return View(assets);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi xảy ra: " + ex.Message);
                return StatusCode(500, "Có lỗi xảy ra khi tải danh sách tài sản.");
            }
        }


        
        // GET method
        [HttpGet]
        public IActionResult AddAsset(Guid idHouse)
        {
            // Lấy danh sách các phòng từ database theo idHouse
            var rooms = _context.Rooms.Where(r => r.HouseId == idHouse).ToList();

            // Truyền danh sách phòng và idHouse vào ViewBag để hiển thị trong view
            ViewBag.Rooms = rooms;
            ViewBag.IdHouse = idHouse;

            // Khởi tạo AssetViewModel nếu cần
            //var model = new AssetViewModel();

            return PartialView("AddAsset");
        }


        // POST method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddAsset(AssetViewModel model, Guid idHouse)
        {
            if (idHouse == Guid.Empty)
            {
                return BadRequest("ID nhà trọ không hợp lệ.");
            }

            // Kiểm tra nếu phòng tồn tại
            var room = _context.Rooms.FirstOrDefault(r => r.Id == model.Asset.RoomId);
            if (room == null)
            {
                return BadRequest("Phòng không tồn tại.");
            }
            if (string.IsNullOrEmpty(model.Asset.Name))
            {
                return BadRequest("Tên tài sản không được để trống.");
            }

            var newAsset = new Asset
            {
                Id = Guid.NewGuid(),
                Name = model.Asset.Name,
                Price = model.Asset.Price,
                Status = model.Asset.Status,
                Number = 1,
                RoomId = model.Asset.RoomId,
                CreateAt = DateTime.Now
            };

            _context.Assets.Add(newAsset);
            _context.SaveChanges();

            return RedirectToAction("AssetMain", new { idHouse });
        }

        // GET: Chỉnh sửa tài sản
        [HttpGet]
        public IActionResult EditAsset(Guid id)
        {
            if (id == Guid.Empty)
            {
                return Json(new { success = false, message = "ID tài sản không hợp lệ." });
            }

            var asset = _context.Assets.FirstOrDefault(a => a.Id == id);
            if (asset == null)
            {
                return Json(new { success = false, message = "Tài sản không tồn tại." });
            }

            // Lấy thông tin phòng từ RoomId của tài sản
            var room = _context.Rooms.FirstOrDefault(r => r.Id == asset.RoomId);
            if (room != null)
            {
                // Lấy danh sách các phòng cùng với nhà trọ mà phòng đó thuộc về
                ViewBag.Rooms = _context.Rooms.Where(r => r.HouseId == room.HouseId).ToList();
            }
            else
            {
                ViewBag.Rooms = new List<KhoaLuan_QLNhaTro.Models.Room>(); // Nếu không có phòng
            }

            // Truyền IdHouse vào ViewBag
            ViewBag.IdHouse = room?.HouseId; // Truyền IdHouse từ phòng nếu có, nếu không thì null

            // Truyền tài sản và thông tin phòng đã chọn vào View
            ViewBag.SelectedRoomId = asset.RoomId;

            return PartialView("EditAsset", asset);
        }



        // POST: Cập nhật tài sản
        [HttpPost]
        public IActionResult EditAsset(Asset asset)
        {
            // Nạp Asset cùng với Room để truy cập HouseId
            var existingAsset = _context.Assets
                .Include(a => a.Room) // Nạp Room để truy cập HouseId
                .FirstOrDefault(a => a.Id == asset.Id);

            if (existingAsset == null)
            {
                TempData["ErrorMessage"] = "Tài sản không tồn tại.";
                return RedirectToAction("AssetMain", new { idHouse = Guid.Empty });
            }

            // Cập nhật thông tin tài sản
            existingAsset.Name = asset.Name;
            existingAsset.Price = asset.Price;
            existingAsset.Status = asset.Status;
            existingAsset.RoomId = asset.RoomId;

            // Lưu thay đổi vào cơ sở dữ liệu
            _context.SaveChanges();

            // Reload Room để đảm bảo dữ liệu đầy đủ
            var updatedRoom = _context.Rooms.FirstOrDefault(r => r.Id == existingAsset.RoomId);

            // Kiểm tra Room và HouseId
            if (updatedRoom != null && updatedRoom.HouseId.HasValue)
            {
                return RedirectToAction("AssetMain", new { idHouse = updatedRoom.HouseId });
            }
            else
            {
                TempData["ErrorMessage"] = "Không tìm thấy nhà trọ hợp lệ.";
                return RedirectToAction("AssetMain", new { idHouse = Guid.Empty });
            }
        }

        [HttpDelete]
        public IActionResult DeleteAsset(Guid id)
        {
            var asset = _context.Assets.Find(id);
            if (asset == null)
            {
                return NotFound(); // Dịch vụ không tồn tại
            }

            _context.Assets.Remove(asset);
            _context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu

            return Ok(); // Trả về trạng thái thành công
        }
    }
}
