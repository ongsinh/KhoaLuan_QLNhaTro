using KhoaLuan_QLNhaTro.Models;
using KhoaLuan_QLNhaTro.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KhoaLuan_QLNhaTro.Controllers
{
    public class UserController : BaseController
    {
        //private readonly NhaTroDbContext _context;
        //public UserController(NhaTroDbContext context)
        //{
        //    _context = context;
        //}

        public UserController(NhaTroDbContext context) : base(context)
        {
        }
        //public IActionResult UserMain()
        //{
        //    var users = _context.Users
        //        .Include(u => u.Account)
        //        .ThenInclude(a => a.Role)
        //        .ToList();
        //    return View(users);
        //}
        public IActionResult UserMain(Guid houseId)
        {
            // Lấy danh sách khách thuê theo HouseId
            var users = _context.Users
                .Include(u => u.Account)
                .Where(u => u.Account.User.Room.HouseId == houseId)
                .ToList();

            // Tạo ViewModel
            var viewModel = new UserHouseViewModel
            {
                IdHouse = houseId,
                Users = users
            };

            // Trả về view và truyền ViewModel
            return View(viewModel);
        }
    }
}
