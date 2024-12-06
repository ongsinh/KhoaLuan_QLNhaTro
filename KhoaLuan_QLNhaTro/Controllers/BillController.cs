using KhoaLuan_QLNhaTro.Models;
using KhoaLuan_QLNhaTro.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
                        Quantity = db.Number.GetValueOrDefault(0), // Số lượng dịch vụ
                        UnitPrice = db.Price, // Đơn giá dịch vụ
                        TotalPrice = db.OldNumber.HasValue && db.NewNumber.HasValue
                            ? (db.NewNumber.Value - db.OldNumber.Value) * db.Price // (Số cuối - Số đầu) * Đơn giá
                            : db.Number.GetValueOrDefault(0) * db.Price // Hoặc tính theo Số lượng * Đơn giá nếu không có Số đầu/Số cuối
                    }).ToList(),
                    TotalBill = (g.FirstOrDefault() != null ? g.FirstOrDefault().Bill.Room.Price : 0)
                                + g.Sum(db => db.OldNumber.HasValue && db.NewNumber.HasValue
                                    ? (db.NewNumber.Value - db.OldNumber.Value) * db.Price
                                    : db.Number.GetValueOrDefault(0) * db.Price) // Tổng tiền hóa đơn (Tiền phòng + Tiền dịch vụ)
                })
                .ToList();

            ViewBag.Services = services;
            ViewBag.Rooms = rooms;
            return View(bills);
        }

        //[HttpGet]
        //public JsonResult GetRoomServices(Guid roomId)
        //{
        //    var services = _context.RoomsServices
        //        .Where(rs => rs.RoomId == roomId)
        //        .Select(rs => new
        //        {
        //            id = rs.Service.Id,
        //            name = rs.Service.Name,
        //            price = rs.Service.Price,
        //            unit = rs.Service.Unit
        //        })
        //        .ToList();

        //    return Json(services);
        //}
        [HttpGet]
        public IActionResult GetRoomServices(Guid roomId)
        {
            var userId = _context.Rooms
                    .Where(r => r.Id == roomId)
                    .Select(r => r.UserId)
                    .FirstOrDefault();
            // Lấy hóa đơn gần nhất của phòng
            var latestBill = _context.Bills
                                     .Where(b => b.RoomId == roomId && b.UserId == userId)
                                     .OrderByDescending(b => b.CreateAt)
                                     .FirstOrDefault();

            if (latestBill == null)
            {
                return Json(new { message = "Không có hóa đơn cho phòng này." });
            }

            // Lấy danh sách dịch vụ của hóa đơn gần nhất
            var services = (from billDetail in _context.DetailBills
                            join service in _context.Services on billDetail.ServiceId equals service.Id
                            where billDetail.BillId == latestBill.Id
                            select new
                            {
                                id =service.Id,
                                name =service.Name,
                                price =service.Price,
                                unit = service.Unit,
                                number = billDetail.Number,
                                OldNumber = billDetail.NewNumber,  // Lấy số cũ từ BillDetail
                            }).ToList();
            if(services == null || services.Count == 0)
            {
                var service = _context.RoomsServices
                .Where(rs => rs.RoomId == roomId)
                .Select(rs => new
                {
                    id = rs.Service.Id,
                    name = rs.Service.Name,
                    price = rs.Service.Price,
                    unit = rs.Service.Unit
                })
                .ToList();
                return Json(service);
            }
            return Json(services);
        }


        //[HttpGet]
        //public JsonResult GetRoomServices(Guid roomId)
        //{
        //    var lastInvoice = _context.Bills
        //.Where(i => i.RoomId == roomId)
        //.OrderByDescending(i => i.CreateAt)  // Lấy hóa đơn mới nhất
        //.Select(i => new
        //{
        //    BillId = i.Id,  // Lấy ID hóa đơn
        //    Services = _context.DetailBills
        //        .Where(d => d.BillId == i.Id)  // Lấy các dịch vụ trong hóa đơn này
        //        .Select(d => new
        //        {
        //            ServiceId = d.ServiceId,
        //            //ServiceName = d.Service.Name,  // Lấy tên dịch vụ
        //            OldNumber = d.NewNumber ?? 0,  // Dữ liệu số cũ từ hóa đơn trước
        //            Number = d.Number ?? 0
        //        }).ToList()
        //})
        //.FirstOrDefault();

        //    if (lastInvoice == null)
        //    {
        //        return Json(new { success = true, services = new List<object>() });
        //    }

        //    return Json(new { success = true, services = lastInvoice.Services });
        //}



        [HttpPost]
        public ActionResult CreateInvoice(string RoomId,DateTime SettlementDate, DateTime CreateAt, DateTime PaymentDate, string ServicesData)
        {
            if (!string.IsNullOrEmpty(ServicesData))
            {
                // Chuyển chuỗi JSON thành danh sách đối tượng
                var services = JsonConvert.DeserializeObject<List<DetailBill>>(ServicesData);

                // Tạo hóa đơn mới
                // Lấy ID người dùng đang đăng nhập
                    var userId = _context.Rooms
                    .Where(r => r.Id == Guid.Parse(RoomId))
                    .Select(r => r.UserId)
                    .FirstOrDefault();

                //    // Tạo hóa đơn mới
                    var bill = new Bill
                    {
                        Id = Guid.NewGuid().ToString(),
                        RoomId = Guid.Parse(RoomId),
                        CreateAt = CreateAt,
                        PaymentDate = PaymentDate,
                        UserId = userId.Value,
                        Status = "False", // Trạng thái "Chưa thanh toán"
                        Total = 0, // Sẽ tính toán sau
                        SettlementDate = SettlementDate,
                    };

                //    // Thêm hóa đơn vào cơ sở dữ liệu
                    _context.Bills.Add(bill);
                    _context.SaveChanges();


                // Lưu chi tiết hóa đơn
                foreach (var service in services)
                {
                    decimal total;

                    // Check if 'Number' is null, calculate accordingly
                    if (service.Number == null)
                    {
                        total = (service.NewNumber.Value - service.OldNumber.Value) * service.Price;
                    }
                    else
                    {
                        total = service.Number.Value * service.Price;
                    }
                    var detailBill = new DetailBill
                    {
                        BillId = bill.Id,
                        SettlementDate = SettlementDate,
                        ServiceId = service.ServiceId,
                        OldNumber = service.OldNumber,
                        NewNumber = service.NewNumber,
                        Number = service.Number,
                        Price = service.Price,
                        Total = total,
                    };

                    _context.DetailBills.Add(detailBill);

                }
                _context.SaveChanges();
            }

            return RedirectToAction("BillMain");
        }


    }

}
