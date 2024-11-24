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

        public IActionResult Index()
        {
            // Lấy danh sách hợp đồng cùng thông tin phòng và người dùng
            var contracts = _context.Contracts
                .Select(c => new
                {
                    RoomName = c.Room.Name,
                    Representative = c.User.Name,
                    //NumberOfMembers = c.Room.NumberOfMembers,
                    RentPrice = c.Room.Price,
                    Deposite = c.Deposit,
                    CreatedDate = c.CreateAt,
                    MoveInDate = c.StartDate,
                    Duration = $"{c.Time} tháng",
                    Status = c.Status
                }).ToList();

            return View(contracts); // Gửi danh sách đến View
        }
    }
}
