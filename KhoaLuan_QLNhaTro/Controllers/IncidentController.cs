using KhoaLuan_QLNhaTro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KhoaLuan_QLNhaTro.Controllers
{
    public class IncidentController : BaseController
    {
        public IncidentController(NhaTroDbContext context) : base(context)
        {
        }
        // Lấy tất cả các sự cố của một nhà trọ
        public async Task<IActionResult> IncidentMain(Guid idHouse)
        {
            Console.WriteLine($"IncidentMain called with idHouse: {idHouse}");

            if (idHouse == Guid.Empty)
            {
                Console.WriteLine("Invalid house ID.");
                return BadRequest("Invalid house ID.");
            }

            // Lấy danh sách các phòng thuộc nhà trọ
            var roomIds = await _context.Rooms
                .Where(r => r.HouseId == idHouse)
                .Select(r => r.Id)
                .ToListAsync();

            if (!roomIds.Any())
            {
                ViewBag.Message = "No rooms found for this house.";
                return View(new List<IncidentRoom>());
            }

            // Lấy danh sách sự cố và thông tin phòng
            var incidents = await _context.IncidentRooms
                .Where(ir => roomIds.Contains(ir.RoomId))
                .Include(ir => ir.Incident) // Bao gồm thông tin sự cố
                .Include(ir => ir.Room)    // Bao gồm thông tin phòng
                .OrderByDescending(ir => ir.Incident.Date) // Sắp xếp theo ngày sự cố
                .ToListAsync();

            if (!incidents.Any())
            {
                ViewBag.Message = "No incidents found for this house.";
            }

            return View(incidents);
        }
    }
}
