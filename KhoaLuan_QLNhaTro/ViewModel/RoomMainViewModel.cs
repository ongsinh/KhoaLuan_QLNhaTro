using KhoaLuan_QLNhaTro.Models;

namespace KhoaLuan_QLNhaTro.ViewModel
{
    public class RoomMainViewModel
    {
        public string HouseName { get; set; }
        public string HouseAddress { get; set; }
        public Guid HouseId { get; set;}
        public House House { get; set; }
        public List<Room> Rooms { get; set; }
    }

}
