namespace KhoaLuan_QLNhaTro.Models.ViewModel
{
    public class HistoryViewModel
    {
        public string billID { get; set; }
        public string TransactionId { get; set; }
        public string OrderDescription { get; set; }
        public decimal Total { get; set; }
        public string PaymentMethod { get; set; }
        public bool Success { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
