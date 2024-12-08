using KhoaLuan_QLNhaTro.Models;

namespace KhoaLuan_QLNhaTro.ViewModel
{
    public class ContractViewModel
    {
        public Guid HouseId { get; set; }
        public Contract Contract { get; set; }

        // Danh sách phòng có sẵn để chọn
        public List<Room> Rooms { get; set; }
        // Thêm thông tin người thuê vào ViewModel
        public User User { get; set; }
    }
}
