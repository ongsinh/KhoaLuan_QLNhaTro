using KhoaLuan_QLNhaTro.Models;

namespace KhoaLuan_QLNhaTro.ViewModel
{
    public class ServiceMainModel
    {
        public string Name { get; set; }
        public RoomServiceViewModel RoomService { get; set; }
        
        public List<DetailBill> DetailBillList { get; set; }
    }
}
