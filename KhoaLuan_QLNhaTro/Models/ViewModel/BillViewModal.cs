using KhoaLuan_QLNhaTro.Models.ViewModel;

namespace KhoaLuan_QLNhaTro.Models
{
    public class BillViewModal
    {
        public string BillId { get; set; }
        public string RoomName { get; set; }
        public decimal RoomPrice { get; set; }
        public string Status { get; set; }
        public List<DetailBillViewModel> Services { get; set; }
        public List<Room> Rooms { get; set; }
        public decimal TotalBill { get; set; }
    }
}
