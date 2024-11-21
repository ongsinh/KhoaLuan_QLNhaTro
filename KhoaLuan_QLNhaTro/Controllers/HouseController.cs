using KhoaLuan_QLNhaTro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace KhoaLuan_QLNhaTro.Controllers
{
    public class HouseController : Controller
    {
        private readonly NhaTroDbContext _context;

        public HouseController(NhaTroDbContext context)
        {
            _context = context;
        }
        
        public IActionResult HouseMain()
        {
            return View();
        }

        [HttpPost]
        [Route("/House/CreateHouse")]
        public IActionResult CreateHouse(string Name, string Address, int floorNumber, List<int> floorRooms)
        {
            if (ModelState.IsValid)
            {
                var house = new House
                {
                    Id = Guid.NewGuid(),
                    Name = Name,
                    Address = Address,
                    floorNumber = floorNumber,
                    CreateAt = DateTime.Now,
                    UpdateAt = DateTime.Now
                };

                _context.Houses.Add(house);
                _context.SaveChanges();

                int roomCount = 1;
                for (int i = 0; i < floorRooms.Count; i++)
                {
                    int roomsInFloor = floorRooms[i]; // Số phòng ở tầng i
                    for (int j = 0; j < roomsInFloor; j++)
                    {
                        var room = new Room
                        {
                            Id = Guid.NewGuid(),
                            Name = $"Tầng {i + 1} - Phòng {roomCount++}",
                            FLoorNumber = i + 1,
                            HouseId = house.Id,
                            CreateAt = DateTime.Now,
                            UpdateAt = DateTime.Now
                        };

                        _context.Rooms.Add(room);
                    }
                }

                _context.SaveChanges();
                return RedirectToAction("HouseMain");
            }

            return View();
        }

    }
}
