using KhoaLuan_QLNhaTro.Models;
using KhoaLuan_QLNhaTro.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KhoaLuan_QLNhaTro.Controllers
{
    public class BillController : Controller
    {
        private readonly NhaTroDbContext _context;

        public IActionResult Index()
        {
            return View();
        }

        public BillController(NhaTroDbContext context)
        {
            _context = context;
        }

        //public IActionResult InvoiceList()
        //{
        //    var services = _context.Services.ToList();
        //    // Lấy danh sách hóa đơn, bao gồm phòng và dịch vụ liên quan
        //    var bills = _context.DetailBills
        //        .Include(db => db.Bill) // Tải thông tin hóa đơn
        //        .ThenInclude(b => b.Room) // Tải thông tin phòng trong hóa đơn
        //        .Include(db => db.Service) // Tải thông tin dịch vụ
        //        .GroupBy(db => db.BillId) // Gom nhóm theo hóa đơn
        //        .Select(g => new
        //        {
        //            BillId = g.Key, // ID hóa đơn
        //            RoomName = g.FirstOrDefault() != null ? g.FirstOrDefault().Bill.Room.Name : "Chưa xác định", // Tên phòng
        //            RoomPrice = g.FirstOrDefault() != null ? g.FirstOrDefault().Bill.Room.Price : 0, // Giá phòng
        //            Status = g.FirstOrDefault() != null ? g.FirstOrDefault().Bill.Status : "Chưa xác định", // Trạng thái hóa đơn
        //            ServiceDetails = g.Select(db => new
        //            {
        //                ServiceName = db.Service.Name, // Tên dịch vụ
        //                ServicePrice = db.Price, // Đơn giá dịch vụ
        //                Quantity = db.Number, // Số lượng dịch vụ
        //                TotalPrice = db.Number * db.Price // Tổng tiền dịch vụ
        //            }).ToList(),
        //            TotalBill = (g.FirstOrDefault() != null ? g.FirstOrDefault().Bill.Room.Price : 0) +
        //                        g.Sum(db => db.Number * db.Price) // Tổng tiền hóa đơn (Tiền phòng + Tiền dịch vụ)
        //        })
        //        .ToList();

        //    ViewBag.Services = services;
        //    // Truyền danh sách hóa đơn vào View
        //    return View(bills);
        //}

        public IActionResult BillMain()
        {
            var services = _context.Services.ToList();
            var rooms = _context.Rooms.ToList();
            // Lấy tất cả hóa đơn và các dịch vụ liên quan thông qua bảng DetailBill
            var bills = _context.DetailBills
                .Include(db => db.Bill)  // Lấy thông tin hóa đơn
                .Include(db => db.Service)  // Lấy thông tin dịch vụ
                .GroupBy(db => db.BillId)  // Nhóm theo BillId để lấy thông tin mỗi hóa đơn
                .Select(g => new BillViewModal
                {
                    BillId = g.Key,
                    RoomName = g.FirstOrDefault() != null ? g.FirstOrDefault().Bill.Room.Name : "Chưa xác định", // Lấy tên phòng từ bản ghi đầu tiên
                    RoomPrice = g.FirstOrDefault() != null ? g.FirstOrDefault().Bill.Room.Price : 0, // Lấy giá phòng
                    Status = g.FirstOrDefault() != null ? g.FirstOrDefault().Bill.Status : "Chưa xác định", // Trạng thái hóa đơn
                    Services = g.Select(db => new DetailBillViewModel
                    {
                        ServiceName = db.Service.Name,  // Tên dịch vụ
                        Quantity = db.Number, // Số lượng dịch vụ
                        UnitPrice = db.Price, // Đơn giá dịch vụ
                        TotalPrice = db.Number * db.Price  // Tính tổng tiền dịch vụ (số lượng * đơn giá)
                    }).ToList(),
                    TotalBill = (g.FirstOrDefault() != null ? g.FirstOrDefault().Bill.Room.Price : 0) + g.Sum(db => db.Number * db.Price) // Tổng tiền hóa đơn (Tiền phòng + Tiền dịch vụ)
                })
                .ToList();

            ViewBag.Services = services;
            ViewBag.Rooms = rooms;
            return View(bills);
        }

        [HttpGet]
        public JsonResult GetRoomServices(Guid roomId)
        {
            var services = _context.RoomsServices
                .Where(rs => rs.RoomId == roomId)
                .Select(rs => new
                {
                    id = rs.Service.Id,
                    name = rs.Service.Name,
                    price = rs.Service.Price,
                    unit = rs.Service.Unit
                })
                .ToList();

            return Json(services);
        }

        [HttpPost]
        public IActionResult CreateInvoice(string RoomId, DateTime CreateAt, DateTime PaymentDate, List<DetailBill> services)
        {
            var userId = _context.Users.Select(u => u.Id).FirstOrDefault();
            var bill = new Bill
                {
                    Id = Guid.NewGuid().ToString(),
                    RoomId = Guid.Parse(RoomId),
                    CreateAt = CreateAt,
                    PaymentDate = PaymentDate,
                    UserId = userId,
                    Status = "False",
                };

                _context.Bills.Add(bill);
                _context.SaveChanges();

                foreach (var service in services)
                {
                    var billService = new DetailBill
                    {
                        BillId = bill.Id,
                        ServiceId = service.ServiceId,
                        OldNumber = service.OldNumber ?? 0,
                        NewNumber = service.NewNumber ?? 0,
                        Number = service.Number
                    };
                    _context.DetailBills.Add(billService);
                }

                _context.SaveChanges();

                return Json(bill);
            }



    }

}
