namespace KhoaLuan_QLNhaTro.Models.ViewModel
{
    public class BillViewModel
    {
        public string RoomId { get; set; } // ID phòng
        public DateTime CreateAt { get; set; } // Ngày tạo hóa đơn
        public DateTime PaymentDate { get; set; } // Hạn thanh toán
        public List<DetailBill> Services { get; set; } // Danh sách dịch vụ chi tiết
    }
}
