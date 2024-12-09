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

        public async Task<IActionResult> IncidentMain(Guid idHouse)
        {
            Console.WriteLine($"IncidentMain called with idHouse: {idHouse}");

            // Check if idHouse is empty
            if (idHouse == Guid.Empty)
            {
                Console.WriteLine("Invalid house ID.");
                return BadRequest("Invalid house ID.");
            }

            // Get all rooms in the house
            var roomIds = await _context.Rooms
                .Where(r => r.HouseId == idHouse)
                .Select(r => r.Id)
                .ToListAsync();

            if (!roomIds.Any())
            {
                ViewBag.Message = "No rooms found for this house.";
                return View(new List<IncidentRoom>());
            }

            Console.WriteLine($"Found room IDs for house {idHouse}: {string.Join(", ", roomIds)}");

            // Get incidents linked to these rooms
            var incidents = await _context.IncidentRooms
                .Where(ir => roomIds.Contains(ir.RoomId)) // Make sure it's linked to rooms of the correct house
                .Include(ir => ir.Incident) // Include incident details
                .Include(ir => ir.Room)    // Include room details
                .OrderByDescending(ir => ir.Incident.Date) // Sort incidents by date
                .ToListAsync();

            if (!incidents.Any())
            {
                ViewBag.Message = "No incidents found for this house.";
                Console.WriteLine("No incidents found.");
            }
            else
            {
                Console.WriteLine($"Found incidents: {incidents.Count}");
            }

            return View(incidents);
        }






        // Lấy tất cả các sự cố của một nhà trọ
        //public async Task<IActionResult> IncidentMain(Guid idHouse)
        //{
        //    Console.WriteLine($"IncidentMain called with idHouse: {idHouse}");

        //    if (idHouse == Guid.Empty)
        //    {
        //        Console.WriteLine("Invalid house ID.");
        //        return BadRequest("Invalid house ID.");
        //    }

        //    // Lấy danh sách các phòng thuộc nhà trọ
        //    var roomIds = await _context.Rooms
        //        .Where(r => r.HouseId == idHouse)
        //        .Select(r => r.Id)
        //        .ToListAsync();

        //    if (!roomIds.Any())
        //    {
        //        ViewBag.Message = "No rooms found for this house.";
        //        return View(new List<IncidentRoom>());
        //    }

        //    // Lấy danh sách sự cố và thông tin phòng
        //    var incidents = await _context.IncidentRooms
        //        .Where(ir => roomIds.Contains(ir.RoomId))
        //        .Include(ir => ir.Incident) // Bao gồm thông tin sự cố
        //        .Include(ir => ir.Room)    // Bao gồm thông tin phòng
        //        .OrderByDescending(ir => ir.Incident.Date) // Sắp xếp theo ngày sự cố
        //        .ToListAsync();

        //    if (!incidents.Any())
        //    {
        //        ViewBag.Message = "No incidents found for this house.";
        //    }

        //    return View(incidents);
        //}
    }
}
