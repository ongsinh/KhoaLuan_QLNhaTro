using KhoaLuan_QLNhaTro.Models;

namespace KhoaLuan_QLNhaTro.ViewModel
{
    public class RoomServiceViewModel
    {
        public Service Service { get; set; } // Dịch vụ
        public Room Room { get; set; } //tên phòng
        /// </summary>
        public List<Room> Rooms { get; set; } // Danh sách phòng áp dụng dịch vụ
    }
}
