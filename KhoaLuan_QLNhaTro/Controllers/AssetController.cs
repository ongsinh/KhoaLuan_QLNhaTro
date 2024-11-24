using KhoaLuan_QLNhaTro.Models;
using KhoaLuan_QLNhaTro.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KhoaLuan_QLNhaTro.Controllers
{
    public class AssetController : Controller
    {
        private readonly NhaTroDbContext _context;

        public AssetController(NhaTroDbContext context)
        {
            _context = context;
        }

        // Action để hiển thị danh sách tài sản
        public IActionResult AssetMain()
        {
            var assets = _context.Assets.Include(a => a.Room).ToList();
            ViewBag.Rooms = _context.Rooms.ToList();
            return View(assets);
        }

        public IActionResult AddAsset()
        {
            // Lấy tất cả các phòng từ cơ sở dữ liệu
            //var rooms = _context.Rooms.ToList();

            //// Kiểm tra xem danh sách phòng có null hay không
            //if (rooms == null || rooms.Count == 0)
            //{
            //    Console.WriteLine("Không có phòng nào trong cơ sở dữ liệu.");
            //}
            ViewBag.Rooms = _context.Rooms.ToList();
            // Tạo ViewModel và gán dữ liệu
            //var viewModel = new AssetViewModel
            //{
            //    Asset = new Asset(),  // Khởi tạo đối tượng Asset mới
            //    Rooms = rooms         // Gán danh sách phòng vào ViewModel
            //};

            // Trả về View với ViewModel chứa danh sách phòng
            return View(/*viewModel*/);
        }


        [HttpPost]
        public IActionResult AddAsset(Asset asset)
        {

            // Tạo ID mới cho tài sản
            asset.Id = Guid.NewGuid();
            asset.CreateAt = DateTime.Now;  // Lấy thời gian tạo tài sản

            // Thêm tài sản vào cơ sở dữ liệu
            _context.Assets.Add(asset);
            _context.SaveChanges();  // Lưu vào cơ sở dữ liệu

            // Chuyển hướng về trang danh sách tài sản (AssetMain)
            return RedirectToAction("AssetMain");
        }

        // GET: Hiển thị form sửa tài sản
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

            ViewBag.Rooms = _context.Rooms.ToList();
            return PartialView("EditAsset", asset); // View "EditAsset.cshtml" chứa form
        }

        // POST: Cập nhật tài sản
        [HttpPost]
        public IActionResult EditAsset(Asset asset)
        {
            var existingAsset = _context.Assets.FirstOrDefault(a => a.Id == asset.Id);

            if (existingAsset == null)
            {
                // Nếu không tìm thấy tài sản, trả về thông báo lỗi
                TempData["ErrorMessage"] = "Tài sản không tồn tại.";
                return RedirectToAction("AssetMain");  // Redirect về trang AssetMain
            }

            // Cập nhật tài sản
            existingAsset.Name = asset.Name;
            existingAsset.Price = asset.Price;
            existingAsset.Number = asset.Number;
            existingAsset.Status = asset.Status;
            existingAsset.RoomId = asset.RoomId;

            // Lưu thay đổi vào cơ sở dữ liệu
            _context.SaveChanges();

            // Lưu thông báo thành công
            TempData["SuccessMessage"] = "Cập nhật tài sản thành công.";

            // Redirect lại trang AssetMain
            return RedirectToAction("AssetMain");
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
