namespace KhoaLuan_QLNhaTro.Models.ViewModel
{
    public class HistoryDetailViewModel
    {
        public string billID { get; set; }
        public string NameRoom { get; set; }
        public decimal roomPrice { get; set; }
        public List<DetailHoaDonViewModel> Details { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreateAt { get; set; }
        public string TransactionId { get; set; }
        public string OrderDescription { get; set; }
        public decimal Total { get; set; }
        public string PaymentMethod { get; set; }
        public bool Success { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
