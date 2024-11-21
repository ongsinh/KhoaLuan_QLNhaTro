using KhoaLuan_QLNhaTro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KhoaLuan_QLNhaTro.Controllers
{
    public class UserController : Controller
    {
        private readonly NhaTroDbContext _context;
        public UserController(NhaTroDbContext context)
        {
            _context = context;
        }

        public IActionResult UserMain()
        {
            var users = _context.Users
                .Include(u => u.Account)
                .ThenInclude(a => a.Role)
                .ToList();
            return View(users);
        }
    }
}
