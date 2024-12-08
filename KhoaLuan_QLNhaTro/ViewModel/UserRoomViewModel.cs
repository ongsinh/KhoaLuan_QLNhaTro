using KhoaLuan_QLNhaTro.Models;

namespace KhoaLuan_QLNhaTro.ViewModel
{
    public class UserRoomViewModel
    {
        public User User { get; set; }
        public Room Room { get; set; }
        public List<RoomService> Services { get; set; }
    }
}
