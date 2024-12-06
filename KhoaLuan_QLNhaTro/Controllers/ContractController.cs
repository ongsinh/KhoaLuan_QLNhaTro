using KhoaLuan_QLNhaTro.Models;
using Microsoft.AspNetCore.Mvc;

namespace KhoaLuan_QLNhaTro.Controllers
{
    public class ContractController : Controller
    {
        private readonly NhaTroDbContext _context;

        public ContractController(NhaTroDbContext context)
        {
            _context = context;
        }

        public IActionResult ContractMain()
        {
            // Lấy danh sách hợp đồng từ database
            var contracts = _context.Contracts
                .Select(c => new Contract
                {
                    Id = c.Id,
                    StartDate = c.StartDate,
                    Time = c.Time,
                    Status = c.Status,
                    Deposit = c.Deposit,
                    CreateAt = DateTime.Now, // Ngày lập là ngày hiện tại
                    UpdateAt = c.UpdateAt,
                    RoomId = c.RoomId,
                    UserId = c.UserId,
                    Room = c.Room,  // Bao gồm thông tin phòng
                    User = c.User   // Bao gồm thông tin người thuê
                })
                .ToList();

            return View(contracts); // Truyền danh sách hợp đồng vào View
        }
    }
}