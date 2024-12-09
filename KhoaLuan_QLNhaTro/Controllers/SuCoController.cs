using KhoaLuan_QLNhaTro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KhoaLuan_QLNhaTro.Controllers
{
    public class SuCoController : BaseController
    {
        public SuCoController(NhaTroDbContext context) : base(context)
        {
        }
        //    public async Task<IActionResult> SuCoMain()
        //    {
        //        var userId = HttpContext.Session.GetString("UserId");
        //        if (userId == null)
        //        {
        //            return RedirectToAction("Login", "Access");
        //        }

        //        var userGuid = Guid.Parse(userId);

        //        // Lấy danh sách sự cố liên quan đến UserId
        //        var incidents = await _context.IncidentRooms
        //.Where(ir => ir.Room.UserId == userGuid) // Kiểm tra UserId của phòng
        //.Include(ir => ir.Incident) // Bao gồm thông tin sự cố
        //.Include(ir => ir.Room)    // Bao gồm thông tin phòng
        //.OrderByDescending(ir => ir.Incident.CreateAt) // Sắp xếp theo ngày tạo sự cố
        //.ToListAsync();

        //        Console.WriteLine($"Found {incidents.Count} incidents for UserId: {userGuid}");

        //        // Chuyển đổi IncidentRoom thành Incident
        //        var incidentList = incidents.Select(ir => ir.Incident).ToList();

        //        return View(incidentList);

        //    }

        public async Task<IActionResult> SuCoMain()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Access");
            }

            var userGuid = Guid.Parse(userId);
            var incidents = await _context.Incidents
                .Where(i => i.UserId == userGuid)
                .OrderByDescending(i => i.CreateAt)
                .ToListAsync();

            Console.WriteLine($"UserId: {userGuid}, Incidents: {incidents.Count}");
            return View(incidents);
        }

        // Thêm sự cố (GET)
        public IActionResult AddSuCo()
        {
            return View();
        }

        // Thêm sự cố (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSuCo(Incident incident)
        {
            //if (!ModelState.IsValid)
            //{
            //    Console.WriteLine("Model state is invalid.");
            //    return View(incident);
            //}

            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                Console.WriteLine("UserId is null or empty.");
                return RedirectToAction("Login", "Access");
            }

            incident.UserId = Guid.Parse(userId);
            incident.CreateAt = DateTime.Now;
            incident.UpdateAt = DateTime.Now;
            incident.Status = "Pending";

            Console.WriteLine($"Incident details: Name={incident.Name}, Description={incident.Description}, UserId={incident.UserId}, Date={incident.Date}");

            try
            {
                _context.Incidents.Add(incident);
                await _context.SaveChangesAsync();
                Console.WriteLine("Incident added to database successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while adding incident to database: {ex.Message}");
                ModelState.AddModelError("", "Lỗi khi thêm sự cố vào cơ sở dữ liệu.");
                return View(incident);
            }

            return RedirectToAction("SuCoMain");
        }

    }
}
