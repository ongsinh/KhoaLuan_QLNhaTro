using KhoaLuan_QLNhaTro.Models;
using KhoaLuan_QLNhaTro.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KhoaLuan_QLNhaTro.Controllers
{
    public class ContractController : BaseController
    {
        public ContractController(NhaTroDbContext context) : base(context)
        {
        }

        public IActionResult ContractMain(Guid houseId)
        {
            // Lấy danh sách hợp đồng theo nhà
            var contracts = _context.Contracts
                .Where(c => c.Room.HouseId == houseId)
                .Include(c => c.Room)  // Bao gồm Room để truy xuất thông tin phòng
                .Include(c => c.User)  // Bao gồm User để truy xuất thông tin khách thuê
                .ToList();

            // Lặp qua danh sách hợp đồng và tính trạng thái hợp đồng
            foreach (var contract in contracts)
            {
                // Tính ngày kết thúc hợp đồng
                DateTime endDate = contract.StartDate.AddMonths(contract.Time);

                // Kiểm tra nếu ngày hiện tại đã vượt qua ngày kết thúc hợp đồng
                if (DateTime.Now > endDate)
                {
                    contract.Status = "Hết hạn hợp đồng";  // Cập nhật trạng thái nếu hết hạn
                }
                else
                {
                    contract.Status = "Trong hạn hợp đồng";  // Trạng thái "Trong hạn" nếu chưa hết hạn
                }
            }
            // Truyền houseId qua ViewBag
            ViewBag.HouseId = houseId;

            return View(contracts);
        }


        [HttpGet]
        public IActionResult AddContract(Guid idHouse)
        {
            // Kiểm tra nhà có tồn tại không
            var house = _context.Houses.FirstOrDefault(h => h.Id == idHouse);
            if (house == null)
            {
                return NotFound("Không tìm thấy nhà với ID được cung cấp.");
            }

            // Lấy các phòng còn trống cho nhà với idHouse
            var rooms = _context.Rooms
                .Where(r => r.HouseId == idHouse && r.Status == "Còn Trống")
                .ToList();

            if (!rooms.Any())
            {
                return NotFound("Không có phòng còn trống trong nhà này.");
            }

            // Tạo ViewModel để truyền dữ liệu cho view
            var viewModel = new ContractViewModel
            {
                Rooms = rooms,
                HouseId = idHouse
            };

            // Trả về PartialView chứa form
            return PartialView(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddContract(ContractViewModel viewModel)
        {
            Console.WriteLine("Phương thức POST được gọi.");
            // Kiểm tra dữ liệu gửi lên
            Console.WriteLine($"Dữ liệu gửi lên: {viewModel.Contract.StartDate}, {viewModel.Contract.Time}, {viewModel.Contract.Deposit}");

            try
            {
                // Kiểm tra dữ liệu đã được gửi đúng chưa
                Console.WriteLine($"Dữ liệu gửi lên: {viewModel.Contract.StartDate}, {viewModel.Contract.Time}, {viewModel.Contract.Deposit}");

                // Kiểm tra nếu khách thuê đã tồn tại dựa trên số căn cước công dân (CCCD)
                var existingUser = _context.Users
                    .FirstOrDefault(u => u.CCCD == viewModel.Contract.User.CCCD); // Kiểm tra theo CCCD

                User user = new User();

                if (existingUser != null)
                {
                    user = existingUser;
                }
                else
                {
                    var account = new Account
                    {
                        Id = Guid.NewGuid(), 
                        CreatedAt = DateTime.Now,
                        Password = "11111", 
                        Phone = viewModel.Contract.User.Account.Phone, 
                        RoleId = "Role_2", 
                        UpdatedAt = DateTime.Now
                    };

                    _context.Accounts.Add(account);
                    _context.SaveChanges(); 

                    
                    user = new User
                    {
                        Id = Guid.NewGuid(),
                        Name = viewModel.Contract.User.Name, 
                        CCCD = viewModel.Contract.User.CCCD, 
                        Gender = viewModel.Contract.User.Gender, 
                        Email = "không xác định", 
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
                    Status = "Đang trong hạn", 
                    RoomId = viewModel.Contract.RoomId,
                    UserId = user.Id,
                    CreateAt = DateTime.Now,
                    UpdateAt = DateTime.Now
                };

                _context.Contracts.Add(contract);
                _context.SaveChanges();

                var room = _context.Rooms
                    .Include(r => r.Contract) // Đảm bảo hợp đồng được tải
                    .FirstOrDefault(r => r.Id == viewModel.Contract.RoomId);

                if (room != null)
                {
                    // Nếu phòng có hợp đồng, cập nhật các thông tin của hợp đồng
                    if (room.Contract != null)
                    {
                        room.Contract.Deposit = viewModel.Contract.Deposit; // Cập nhật tiền cọc
                        room.Contract.StartDate = viewModel.Contract.StartDate; // Cập nhật ngày bắt đầu
                        room.UserId = user.Id; // Cập nhật UserId
                    }
                    else
                    {
                        // Nếu phòng chưa có hợp đồng, thông báo hoặc xử lý thêm
                        Console.WriteLine("Phòng này chưa có hợp đồng.");
                    }

                    // Cập nhật trạng thái phòng
                    room.Status = "Đang ở";

                    // Lưu thay đổi
                    _context.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Phòng không tồn tại.");
                }


                // Sau khi lưu hợp đồng thành công, chuyển hướng đến ContractMain với houseId
                return RedirectToAction("ContractMain", new { houseId = viewModel.HouseId });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi thêm hợp đồng: {ex.Message}");
                ModelState.AddModelError("Contract", "Đã xảy ra lỗi khi xử lý yêu cầu.");
                return View(viewModel);
            }
        }


        // Phương thức gia hạn hợp đồng
        public IActionResult RenewContract(string contractId)
        {
            // Tìm hợp đồng cũ dựa trên contractId
            var contract = _context.Contracts.FirstOrDefault(c => c.Id == contractId);

            if (contract == null)
            {
                return NotFound("Hợp đồng không tồn tại.");
            }

            // Cập nhật hợp đồng cũ: thay đổi trạng thái thành "Hết hạn"
            contract.Status = "Hết hạn";
            contract.UpdateAt = DateTime.Now;
            _context.SaveChanges();

            // Tạo hợp đồng mới (gia hạn hợp đồng)
            var newContract = new Contract
            {
                Id = Guid.NewGuid().ToString(),
                StartDate = contract.StartDate.AddMonths(contract.Time), // Ngày bắt đầu là ngày sau khi hợp đồng cũ kết thúc
                Time = contract.Time, // Thời gian thuê mới (có thể thay đổi nếu cần)
                Deposit = contract.Deposit, // Tiền cọc (giữ nguyên)
                Status = "Đang trong hạn", // Trạng thái hợp đồng mới
                RoomId = contract.RoomId, // Liên kết phòng với hợp đồng mới
                UserId = contract.UserId, // Liên kết người dùng với hợp đồng mới
                CreateAt = DateTime.Now,
                UpdateAt = DateTime.Now
            };

            // Thêm hợp đồng mới vào cơ sở dữ liệu
            _context.Contracts.Add(newContract);
            _context.SaveChanges();

            // Cập nhật phòng để liên kết với hợp đồng mới
            var room = _context.Rooms.FirstOrDefault(r => r.Id == contract.RoomId);
            if (room != null)
            {
                room.Contract.Id = newContract.Id; // Liên kết phòng với hợp đồng mới
                _context.SaveChanges();
            }

            // Quay lại trang danh sách hợp đồng của nhà
            return RedirectToAction("ContractMain", new { houseId = contract.Room.HouseId });
        }
    }
}
