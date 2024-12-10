using KhoaLuan_QLNhaTro.Models;
using KhoaLuan_QLNhaTro.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Policy;
using VnPayIntegration.Models;

namespace KhoaLuan_QLNhaTro.Controllers
{
    public class BillController : Controller
    {
        private readonly NhaTroDbContext _context;
        private readonly IVnPayService _vnPayService;

        public IActionResult Index()
        {
            return View();
        }
        
        public BillController(NhaTroDbContext context, IVnPayService vnPayService)
        {
            _context = context;
            _vnPayService = vnPayService;
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

        //public IActionResult BillMain(Guid idHouse)
        //{
        //    var services = _context.Services.ToList();
        //    var rooms = _context.Rooms
        //    .Where(r => r.HouseId == idHouse)
        //   .ToList();
        //    ViewBag.Services = services;
        //    ViewBag.Rooms = rooms;
        //    ViewBag.IdHouse = idHouse;
        //    // Lấy tất cả hóa đơn và các dịch vụ liên quan thông qua bảng DetailBill
        //    var bills = _context.DetailBills
        //        .Include(db => db.Bill)  // Lấy thông tin hóa đơn
        //        .Include(db => db.Service)  // Lấy thông tin dịch vụ
        //        .GroupBy(db => db.BillId)  // Nhóm theo BillId để lấy thông tin mỗi hóa đơn
        //        .Select(g => new BillViewModal
        //        {
        //            BillId = g.Key,
        //            RoomName = g.FirstOrDefault() != null ? g.FirstOrDefault().Bill.Room.Name : "Chưa xác định", // Lấy tên phòng từ bản ghi đầu tiên
        //            RoomPrice = g.FirstOrDefault() != null ? g.FirstOrDefault().Bill.Room.Price : 0, // Lấy giá phòng
        //            Status = g.FirstOrDefault() != null ? g.FirstOrDefault().Bill.Status : "Chưa xác định", // Trạng thái hóa đơn
        //            Services = g.Select(db => new DetailBillViewModel
        //            {
        //                ServiceName = db.Service.Name,  // Tên dịch vụ
        //                Quantity = db.Number.GetValueOrDefault(0), // Số lượng dịch vụ
        //                UnitPrice = db.Price, // Đơn giá dịch vụ
        //                TotalPrice = db.OldNumber.HasValue && db.NewNumber.HasValue
        //                    ? (db.NewNumber.Value - db.OldNumber.Value) * db.Price // (Số cuối - Số đầu) * Đơn giá
        //                    : db.Number.GetValueOrDefault(0) * db.Price // Hoặc tính theo Số lượng * Đơn giá nếu không có Số đầu/Số cuối
        //            }).ToList(),
        //            TotalBill = (g.FirstOrDefault() != null ? g.FirstOrDefault().Bill.Room.Price : 0)
        //                        + g.Sum(db => db.OldNumber.HasValue && db.NewNumber.HasValue
        //                            ? (db.NewNumber.Value - db.OldNumber.Value) * db.Price
        //                            : db.Number.GetValueOrDefault(0) * db.Price) // Tổng tiền hóa đơn (Tiền phòng + Tiền dịch vụ)
        //        })
        //        .ToList();


        //    return View(bills);
        //}
        public IActionResult BillMain(Guid idHouse)
        {
            //var services = _context.Services.ToList();
            var services = _context.RoomsServices
                .Where(rs => rs.Room.HouseId == idHouse) // Lọc theo HouseId
                .Select(rs => new
                {
                    rs.Service.Name,
                })
                .Distinct() // Loại bỏ trùng lặp nếu dịch vụ được liên kết với nhiều phòng
                .ToList();

            // Lấy danh sách ID của các phòng thuộc nhà
            var roomIds = _context.Rooms
                .Where(r => r.HouseId == idHouse)
                .Select(r => r.Id)
                .ToList(); // Chỉ lấy danh sách ID để sử dụng trong truy vấn LINQ

            ViewBag.Services = services;
            ViewBag.Rooms = _context.Rooms.Where(r => roomIds.Contains(r.Id)).ToList(); // Lấy lại danh sách phòng

            // Lấy danh sách hóa đơn và dịch vụ liên quan
            var bills = _context.DetailBills
                .Include(db => db.Bill)       // Lấy thông tin hóa đơn
                .ThenInclude(b => b.Room)    // Lấy thông tin phòng từ hóa đơn
                .Include(db => db.Service)   // Lấy thông tin dịch vụ
                .Where(db => roomIds.Contains(db.Bill.RoomId)) // Sử dụng roomIds thay vì rooms.Any(...)
                .GroupBy(db => db.BillId)    // Nhóm theo BillId để lấy thông tin mỗi hóa đơn
                .Select(g => new BillViewModal
                {
                    BillId = g.Key,
                    RoomName = g.FirstOrDefault() != null ? g.FirstOrDefault().Bill.Room.Name : "Chưa xác định", // Lấy tên phòng
                    RoomPrice = g.FirstOrDefault() != null ? g.FirstOrDefault().Bill.Room.Price : 0, // Lấy giá phòng
                    Status = g.FirstOrDefault() != null ? g.FirstOrDefault().Bill.Status : "Chưa xác định", // Trạng thái hóa đơn
                    Services = g.Select(db => new DetailBillViewModel
                    {
                        ServiceName = db.Service.Name,   // Tên dịch vụ
                        Quantity = db.Number.GetValueOrDefault(0), // Số lượng dịch vụ
                        UnitPrice = db.Price,           // Đơn giá dịch vụ
                        TotalPrice = db.OldNumber.HasValue && db.NewNumber.HasValue
                            ? (db.NewNumber.Value - db.OldNumber.Value) * db.Price // (Số cuối - Số đầu) * Đơn giá
                            : db.Number.GetValueOrDefault(0) * db.Price           // Hoặc tính theo Số lượng * Đơn giá nếu không có Số đầu/Số cuối
                    }).ToList(),
                    TotalBill = (g.FirstOrDefault() != null ? g.FirstOrDefault().Bill.Room.Price : 0)
                                + g.Sum(db => db.OldNumber.HasValue && db.NewNumber.HasValue
                                    ? (db.NewNumber.Value - db.OldNumber.Value) * db.Price
                                    : db.Number.GetValueOrDefault(0) * db.Price) // Tổng tiền hóa đơn (Tiền phòng + Tiền dịch vụ)
                })
                .ToList();
            ViewBag.HouseId = idHouse;
            return View(bills);
        }


        [HttpGet]
        public IActionResult GetRoomServices(Guid roomId)
        {
            // Tìm hóa đơn gần nhất theo RoomId
            var latestBill = _context.Bills
                .Where(b => b.RoomId == roomId)
                .OrderByDescending(b => b.CreateAt)
                .FirstOrDefault();

            if (latestBill == null)
            {
                // Nếu không có hóa đơn, lấy danh sách dịch vụ liên quan đến phòng
                var roomServices = _context.RoomsServices
                    .Where(rs => rs.RoomId == roomId)
                    .Select(rs => new
                    {
                        id = rs.Service.Id,
                        name = rs.Service.Name,
                        price = rs.Service.Price,
                        unit = rs.Service.Unit,
                        OldNumber = 0, // Mặc định số cũ
                        NewNumber = 0, // Mặc định số mới
                        Number = 1     // Mặc định số lượng
                    })
                    .ToList();

                return Json(roomServices);
            }

            // Nếu có hóa đơn, lấy dịch vụ từ DetailBill của hóa đơn đó
            var services = (from billDetail in _context.DetailBills
                            join service in _context.Services on billDetail.ServiceId equals service.Id
                            where billDetail.BillId == latestBill.Id
                            select new
                            {
                                id = service.Id,
                                name = service.Name,
                                price = service.Price,
                                unit = service.Unit,
                                OldNumber = billDetail.NewNumber ?? 0, // Lấy số mới của hóa đơn cũ làm số cũ
                                NewNumber = 0, // Số mới để người dùng nhập
                                Number = billDetail.Number ?? 0        // Số lượng cũ
                            }).ToList();

            return Json(services);
        }



        //[HttpGet]
        //public IActionResult GetRoomServices(Guid roomId)
        //{
        //    var userId = _context.Rooms
        //        .Where(r => r.Id == roomId)
        //        .Select(r => r.UserId)
        //        .FirstOrDefault();

        //    // Lấy hóa đơn gần nhất của phòng
        //    var latestBill = _context.Bills
        //        .Where(b => b.RoomId == roomId && b.UserId == userId)
        //        .OrderByDescending(b => b.CreateAt) // Sắp xếp theo thời gian tạo hóa đơn (nếu cần)
        //        .FirstOrDefault();

        //    if (latestBill == null)
        //    {
        //        // Không có hóa đơn, trả về danh sách dịch vụ đang áp dụng cho phòng
        //        var roomServices = _context.RoomsServices
        //            .Where(rs => rs.RoomId == roomId)
        //            .Select(rs => new
        //            {
        //                id = rs.Service.Id,
        //                name = rs.Service.Name,
        //                price = rs.Service.Price,
        //                unit = rs.Service.Unit
        //            })
        //            .ToList();

        //        if (roomServices == null || !roomServices.Any())
        //        {
        //            return Json(new { message = "Phòng này chưa được áp dụng dịch vụ nào." });
        //        }

        //        return Json(roomServices);
        //    }

        //    // Lấy danh sách dịch vụ của hóa đơn gần nhất
        //    var services = (from billDetail in _context.DetailBills
        //                    join service in _context.Services on billDetail.ServiceId equals service.Id
        //                    where billDetail.BillId == latestBill.Id
        //                    select new
        //                    {
        //                        id = service.Id,
        //                        name = service.Name,
        //                        price = service.Price,
        //                        unit = service.Unit,
        //                        number = billDetail.Number,
        //                        oldNumber = billDetail.NewNumber // Số cũ từ DetailBill
        //                    }).ToList();

        //    if (services == null || !services.Any())
        //    {
        //        // Nếu hóa đơn không chứa dịch vụ nào, trả về danh sách dịch vụ đang áp dụng cho phòng
        //        var roomServices = _context.RoomsServices
        //            .Where(rs => rs.RoomId == roomId)
        //            .Select(rs => new
        //            {
        //                id = rs.Service.Id,
        //                name = rs.Service.Name,
        //                price = rs.Service.Price,
        //                unit = rs.Service.Unit
        //            })
        //            .ToList();

        //        return Json(roomServices);
        //    }

        //    return Json(services);
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
                        Status = "Chưa thanh toán", // Trạng thái "Chưa thanh toán"
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


        //public IActionResult CreatePaymentUrlVnpay(PaymentInformationModel model)
        //{
        //    var url = _vnPayService.CreatePaymentUrl(model, HttpContext);
        //    return Redirect(url);
        //}

        [HttpGet]
        public IActionResult PaymentCallbackVnpay()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);
            var billid = response.BillId;
            var total = response.Total;
            if (response.Success)
            {
                var bill = _context.Bills.FirstOrDefault(b => b.Id == billid);
                if (bill != null)
                {
                    bill.Status = "Đã thanh toán";
                    bill.Total = total;

                    // Lưu thay đổi vào cơ sở dữ liệu
                    _context.SaveChanges();
                }
            }
            return View(response); // Trả về view với model PaymentResponseModel
        }

        [HttpPost]
        public IActionResult CreatePaymentUrlVnpay([FromBody] PaymentInformationModel model)
        {
            try
            {
                var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

                // Trả về URL thanh toán để frontend xử lý
                return Json(new { success = true, paymentUrl = url });
            }
            catch (Exception ex)
            {
                // Trả về lỗi nếu có
                return Json(new { success = false, message = ex.Message });
            }
        }




        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetBillDetails(string billId)
        {
            var bill = _context.Bills
                .Where(b => b.Id == billId)
                .Select(b => new
                {
                    b.Id,
                    RoomName = b.Room.Name, // Get Room Name
                    RoomPrice =b.Room.Price, // Get Room Price
                    b.Status,
                    //b.TotalBill,
                    b.CreateAt,
                    b.PaymentDate,
                    Services = _context.DetailBills
                        .Where(dbill => dbill.BillId == b.Id)
                        .Select(dbill => new
                        {
                            dbill.ServiceId,
                            dbill.BillId,
                            ServiceName = dbill.Service.Name,
                            dbill.Number,
                            dbill.Price,
                            dbill.OldNumber, // Số đầu nếu có
                            dbill.NewNumber,   // Số cuối nếu có
                            //TotalPrice = dbill.Quantity * dbill.UnitPrice,
                            //dbill.UsageDetail
                        })
                        .ToList()
                })
                .FirstOrDefault();

            if (bill == null)
            {
                return Json(new { success = false, message = "Hóa đơn không tồn tại!" });
            }

            // Tính tổng tiền của hóa đơn
            decimal totalBill = bill.RoomPrice; // Tiền phòng
            decimal roomPrice = bill.RoomPrice;
            foreach (var service in bill.Services)
            {
                if (service.OldNumber != null && service.NewNumber != null)
                {
                    // Tính tiền nếu có số đầu và số cuối
                    totalBill += (service.NewNumber.Value - service.OldNumber.Value) * service.Price;
                }
                else if (service.Number != null)
                {
                    // Tính tiền nếu có số lượng
                    totalBill += service.Number.Value * service.Price;
                }
            }

            return Json(new { success = true, bill, totalBill , roomPrice });
        }

        public IActionResult UpdateBill(string billId, DateTime createAt, DateTime paymentDate, string servicesData)
        {


            var bill = _context.Bills.Where(b => b.Id == billId).FirstOrDefault();

            // Cập nhật thông tin hóa đơn
            bill.CreateAt = createAt;
            bill.PaymentDate = paymentDate;
            var services = JsonConvert.DeserializeObject<List<DetailBill>>(servicesData);
            foreach (var service in services)
            {
                // Tìm dịch vụ trong cơ sở dữ liệu
                var detailBills = _context.DetailBills
                    .Where(db => db.BillId == billId && db.ServiceId == service.ServiceId)
                    .ToList();

                if (detailBills.Any())
                {
                    // Cập nhật thông tin dịch vụ
                    foreach (var detailBill in detailBills)
                    {
                        detailBill.OldNumber = service.OldNumber;
                        detailBill.NewNumber = service.NewNumber;
                        detailBill.Number = service.Number;
                    }
                }
                else
                {
                    // Nếu không tìm thấy dịch vụ, trả về lỗi
                    return Json(new { success = false, message = $"Dịch vụ với ID {service.ServiceId} không tồn tại trong hóa đơn!" });
                }
            }

            // Lưu thay đổi vào cơ sở dữ liệu
            _context.SaveChanges();

            return Json(new { success = true, message = "Cập nhật hóa đơn và dịch vụ thành công!" });
        }

    }

}
