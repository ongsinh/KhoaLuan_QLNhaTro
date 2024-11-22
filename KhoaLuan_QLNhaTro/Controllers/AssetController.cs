using KhoaLuan_QLNhaTro.Models;
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
            return View(assets);
        }

        public IActionResult AddAsset()
        {
            // Lấy danh sách phòng từ database
            var rooms = _context.Rooms.ToList();

            // Truyền danh sách phòng vào ViewData để hiển thị trong dropdown
            ViewData["Rooms"] = rooms;

            // Trả về PartialView chứa form thêm tài sản
            return PartialView("_AddAsset", new Asset());
        }

        [HttpPost]
        public IActionResult AddAsset(Asset asset)
        {
            if (ModelState.IsValid)
            {
                asset.CreateAt = DateTime.Now;
                asset.UpdateAt = DateTime.Now;

                // Thêm tài sản vào database
                _context.Assets.Add(asset);
                _context.SaveChanges();

                // Sau khi thêm, chuyển hướng lại về trang danh sách tài sản
                return RedirectToAction("Index");
            }

            // Nếu có lỗi, trả về lại PartialView
            var rooms = _context.Rooms.ToList();
            ViewData["Rooms"] = rooms;

            return PartialView("_AddAsset", asset);
        }

    }
}
