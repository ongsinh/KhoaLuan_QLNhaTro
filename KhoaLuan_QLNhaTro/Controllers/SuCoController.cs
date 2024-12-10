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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSuCo(Incident incident)
        {
            // Get the UserId from the session
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                Console.WriteLine("UserId is null or empty.");
                return RedirectToAction("Login", "Access");
            }

            // Parse UserId to Guid
            var userGuid = Guid.Parse(userId);

            // Fetch the contract based on UserId to get the associated RoomId
            var contract = await _context.Contracts
                .Include(c => c.Room) // Include the room details in the contract
                .FirstOrDefaultAsync(c => c.UserId == userGuid);

            if (contract == null || contract.Room == null)
            {
                Console.WriteLine("Contract or Room not found for the user.");
                return NotFound("Không tìm thấy hợp đồng hoặc phòng cho người dùng.");
            }

            // Set the incident properties
            incident.UserId = userGuid;
            incident.CreateAt = DateTime.Now;
            incident.UpdateAt = DateTime.Now;
            incident.Status = "Pending"; // You can set it based on your logic

            try
            {
                // Add the incident to the Incidents table
                _context.Incidents.Add(incident);
                await _context.SaveChangesAsync(); // Save to generate the IncidentId

                Console.WriteLine("Incident added to database successfully.");

                // Now associate the incident with the room using the IncidentRoom table
                var incidentRoom = new IncidentRoom
                {
                    RoomId = contract.Room.Id, // Use the RoomId from the contract
                    IncidentId = incident.Id, // The newly created IncidentId
                    Status = "Pending" // Set status if necessary, or modify based on logic
                };

                // Add the IncidentRoom association
                _context.IncidentRooms.Add(incidentRoom);
                await _context.SaveChangesAsync(); // Save the association

                Console.WriteLine("IncidentRoom entry added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while adding incident or associating with room: {ex.Message}");
                ModelState.AddModelError("", "Lỗi khi thêm sự cố vào cơ sở dữ liệu.");
                return View(incident);
            }

            // Redirect to the SuCoMain view after successful addition
            return RedirectToAction("SuCoMain");
        }

        // Action to get the incident for editing
        // Lấy thông tin sự cố để sửa (Ajax)
        [HttpGet]
        public IActionResult EditSuCo(Guid id)
        {
            var incident = _context.Incidents.FirstOrDefault(x => x.Id == id);
            if (incident == null)
            {
                return NotFound();
            }
            return PartialView("EditSuCo", incident);
        }

        // Lưu thông tin sửa sự cố
        [HttpPost]
        public IActionResult EditSuCo(Incident incident)
        {
            var existingIncident = _context.Incidents.FirstOrDefault(x => x.Id == incident.Id);
            if (existingIncident == null)
            {
                return NotFound();
            }

            existingIncident.Name = incident.Name;
            existingIncident.Description = incident.Description;
            existingIncident.Status = incident.Status;
            existingIncident.Date = incident.Date;
            _context.SaveChanges();

            return RedirectToAction("SuCoMain");
            //return Json(new { success = true });
        }

        // Xóa sự cố
        //[HttpPost]
        //public IActionResult DeleteSuCo(Guid id)
        //{
        //    var incident = _context.Incidents.FirstOrDefault(x => x.Id == id);
        //    if (incident == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Incidents.Remove(incident);
        //    _context.SaveChanges();
        //    return Json(new { success = true });
        //}
        //[HttpPost]
        //public IActionResult DeleteSuCo(Guid id)
        //{
        //    try
        //    {
        //        var incident = _context.Incidents.FirstOrDefault(x => x.Id == id);
        //        if (incident == null)
        //        {
        //            return NotFound();
        //        }

        //        _context.Incidents.Remove(incident);
        //        _context.SaveChanges();
        //        return Json(new { success = true });
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        // Log lỗi chi tiết
        //        Console.WriteLine($"Error: {ex.InnerException?.Message}");
        //        return Json(new { success = false, message = "Không thể xóa sự cố. Hãy kiểm tra lại." });
        //    }
        //}
        [HttpPost]
        public IActionResult DeleteSuCo(Guid id)
        {
            using var transaction = _context.Database.BeginTransaction(); // Đảm bảo tính toàn vẹn giao dịch
            try
            {
                // Tìm và xóa tất cả các bản ghi liên quan trong IncidentRooms
                var relatedRecords = _context.IncidentRooms.Where(ir => ir.IncidentId == id).ToList();
                _context.IncidentRooms.RemoveRange(relatedRecords);

                // Xóa bản ghi trong bảng Incidents
                var incident = _context.Incidents.FirstOrDefault(x => x.Id == id);
                if (incident == null)
                {
                    return NotFound();
                }

                _context.Incidents.Remove(incident);
                _context.SaveChanges();

                transaction.Commit(); // Xác nhận giao dịch
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                transaction.Rollback(); // Hoàn tác nếu có lỗi
                Console.WriteLine($"Error: {ex.Message}");
                return Json(new { success = false, message = "Không thể xóa sự cố." });
            }
        }

    }
}
