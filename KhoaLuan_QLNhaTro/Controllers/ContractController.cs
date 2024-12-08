using KhoaLuan_QLNhaTro.Models;
using KhoaLuan_QLNhaTro.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KhoaLuan_QLNhaTro.Controllers
{
    public class ContractController : BaseController
    {
        //private readonly NhaTroDbContext _context;

        //public ContractController(NhaTroDbContext context)
        //{
        //    _context = context;
        //}

        public ContractController(NhaTroDbContext context) : base(context)
        {
        }
        // Hiển thị danh sách hợp đồng theo idHouse
        public IActionResult ContractMain(Guid houseId)
        {
            // Lấy danh sách hợp đồng theo nhà
            var contracts = _context.Contracts
                .Where(c => c.Room.HouseId == houseId)
                .Include(c => c.Room)  // Bao gồm Room để truy xuất thông tin phòng
                .Include(c => c.User)  // Bao gồm User để truy xuất thông tin khách thuê
                .ToList();

            // Truyền houseId qua ViewBag
            ViewBag.HouseId = houseId;

            return View(contracts);
        }

        // GET: Hiển thị form thêm hợp đồng
        //public IActionResult AddContract(Guid idHouse)
        //{
        //    if (!_context.Houses.Any(h => h.Id == idHouse))
        //    {
        //        return NotFound("Không tìm thấy nhà với ID được cung cấp.");
        //    }

        //    var rooms = _context.Rooms
        //        .Where(r => r.HouseId == idHouse && r.Status == "Còn Trống") // Lọc theo idHouse và trạng thái phòng
        //        .ToList();

        //    if (!rooms.Any())
        //    {
        //        return PartialView("AddContract", new ContractViewModel { Rooms = new List<Room>() });
        //    }

        //    var viewModel = new ContractViewModel
        //    {
        //        Rooms = rooms
        //    };

        //    return PartialView("AddContract", new ContractViewModel { Rooms = new List<Room>() });
        //}

        public IActionResult AddContract(Guid idHouse)
        {
            // Kiểm tra nhà có tồn tại không
            if (!_context.Houses.Any(h => h.Id == idHouse))
            {
                return NotFound("Không tìm thấy nhà với ID được cung cấp.");
            }

            // Lấy các phòng còn trống cho nhà với idHouse
            var rooms = _context.Rooms
                .Where(r => r.HouseId == idHouse && r.Status == "Còn Trống")
                .ToList();

            if (!rooms.Any())
            {
                Console.WriteLine($"Không có phòng nào cho nhà với ID: {idHouse}");
            }

            // Tạo ViewModel để truyền dữ liệu cho view
            var viewModel = new ContractViewModel
            {
                Rooms = rooms,
                HouseId = idHouse
            };

            ViewBag.IdHouse = idHouse; // Truyền HouseId vào ViewBag

            // Trả về PartialView chứa form
            return PartialView("AddContract", viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddContract(ContractViewModel viewModel, Guid houseId)
        {
            houseId = viewModel.HouseId; // Lấy HouseId từ ViewModel
            try
            {
                //if (!ModelState.IsValid)
                //{
                //    return View(viewModel);
                //}

                // Kiểm tra nếu khách thuê đã tồn tại dựa trên số căn cước công dân (CCCD)
                var existingUser = _context.Users
                    .FirstOrDefault(u => u.CCCD == viewModel.Contract.User.CCCD); // Kiểm tra theo CCCD

                User user;

                if (existingUser != null)
                {
                    user = existingUser;
                }
                else
                {
                    var account = new Account
                    {
                        Phone = viewModel.Contract.User.Account.Phone, // Lấy số điện thoại từ form
                    };

                    _context.Accounts.Add(account);
                    _context.SaveChanges(); // Lưu Account trước khi tạo User

                    user = new User
                    {
                        Name = viewModel.Contract.User.Name,
                        CCCD = viewModel.Contract.User.CCCD,
                        Gender = viewModel.Contract.User.Gender,
                        Email = viewModel.Contract.User.Email,
                        Dob = viewModel.Contract.User.Dob,
                        Address = viewModel.Contract.User.Address,
                        AccountId = account.Id,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };

                    _context.Users.Add(user);
                    _context.SaveChanges(); // Lưu User
                }

                var contract = new Contract
                {
                    Id = Guid.NewGuid().ToString(),
                    StartDate = viewModel.Contract.StartDate,
                    Time = viewModel.Contract.Time,
                    Deposit = viewModel.Contract.Deposit,
                    Status = "Đang trong hạn", // Trạng thái mặc định khi thêm hợp đồng
                    RoomId = viewModel.Contract.RoomId,
                    UserId = user.Id,
                    CreateAt = DateTime.Now,
                    UpdateAt = DateTime.Now
                };

                _context.Contracts.Add(contract);
                try
                {
                    _context.SaveChanges();
                    Console.WriteLine("Dữ liệu đã được lưu vào cơ sở dữ liệu.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi lưu hợp đồng: {ex.Message}");
                }
                
                Console.WriteLine("Hợp đồng đã được thêm thành công.");
                return RedirectToAction("ContractMain", new { houseId = houseId });
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi thêm hợp đồng: {ex.Message}");
                ModelState.AddModelError("Contract", "Đã xảy ra lỗi khi xử lý yêu cầu.");
                return View(viewModel);
            }
        }


        // POST: Xử lý việc lưu hợp đồng
        //[HttpPost]
        //public IActionResult AddContract(ContractViewModel viewModel, Guid houseId)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            // Nếu dữ liệu không hợp lệ, trả lại form
        //            return View(viewModel);
        //        }

        //        // Kiểm tra nếu khách thuê đã tồn tại dựa trên số căn cước công dân (CCCD)
        //        var existingUser = _context.Users
        //            .FirstOrDefault(u => u.CCCD == viewModel.Contract.User.CCCD); // Kiểm tra theo CCCD

        //        User user;

        //        if (existingUser != null)
        //        {
        //            // Nếu khách thuê đã tồn tại, lấy thông tin khách thuê đó
        //            user = existingUser;
        //        }
        //        else
        //        {
        //            // Nếu khách thuê chưa tồn tại, tạo mới User và Account
        //            var account = new Account
        //            {
        //                Phone = viewModel.Contract.User.Account.Phone, // Lấy số điện thoại từ form
        //            };

        //            // Thêm Account vào DB
        //            _context.Accounts.Add(account);
        //            _context.SaveChanges(); // Lưu Account trước khi tạo User để lấy AccountId

        //            // Tạo User
        //            user = new User
        //            {
        //                Name = viewModel.Contract.User.Name,
        //                CCCD = viewModel.Contract.User.CCCD,
        //                Gender = viewModel.Contract.User.Gender,
        //                Email = viewModel.Contract.User.Email,
        //                Dob = viewModel.Contract.User.Dob,
        //                Address = viewModel.Contract.User.Address,
        //                AccountId = account.Id, // Liên kết với Account đã tạo
        //                CreatedAt = DateTime.Now,
        //                UpdatedAt = DateTime.Now
        //            };

        //            // Thêm User vào DB
        //            _context.Users.Add(user);
        //            _context.SaveChanges(); // Lưu User để có UserId
        //        }

        //        // Tạo Contract (hợp đồng)
        //        var contract = new Contract
        //        {
        //            Id = Guid.NewGuid().ToString(),
        //            StartDate = viewModel.Contract.StartDate,
        //            Time = viewModel.Contract.Time,
        //            Deposit = viewModel.Contract.Deposit,
        //            Status = "Đang trong hạn", // Trạng thái mặc định khi thêm mới hợp đồng
        //            RoomId = viewModel.Contract.RoomId, // Lấy ID phòng từ form
        //            UserId = user.Id, // Lấy UserId đã tạo hoặc đã tồn tại
        //            CreateAt = DateTime.Now,
        //            UpdateAt = DateTime.Now
        //        };

        //        // Thêm hợp đồng vào cơ sở dữ liệu
        //        _context.Contracts.Add(contract);
        //        var rowsAffected = _context.SaveChanges();

        //        if (rowsAffected > 0)
        //        {
        //            // Sau khi thêm hợp đồng thành công, chuyển hướng về trang danh sách hợp đồng
        //            return RedirectToAction("ContractMain", new { houseId = houseId });
        //        }
        //        else
        //        {
        //            // Nếu không có bản ghi nào được thêm vào
        //            ModelState.AddModelError("Contract", "Lỗi khi lưu hợp đồng. Vui lòng thử lại.");
        //            return View(viewModel);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Ghi log và hiển thị thông báo lỗi
        //        ModelState.AddModelError("Contract", "Đã xảy ra lỗi khi xử lý yêu cầu: " + ex.Message);
        //        return View(viewModel);
        //    }
        //}
    }
}
