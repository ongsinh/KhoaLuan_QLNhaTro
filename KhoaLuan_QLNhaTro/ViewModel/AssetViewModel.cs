using KhoaLuan_QLNhaTro.Models;

namespace KhoaLuan_QLNhaTro.ViewModel
{
    public class AssetViewModel
    {
        public Asset Asset { get; set; } = new Asset(); // Dữ liệu tài sản
        public List<Room> Rooms { get; set; } // Danh sách phòng
        public Guid IdHouse { get; set; }
    }
}
