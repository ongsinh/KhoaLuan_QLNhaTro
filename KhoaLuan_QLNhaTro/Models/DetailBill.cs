namespace KhoaLuan_QLNhaTro.Models
{
    public class DetailBill
    {
        public string BillId { get; set; }
        public Guid ServiceId { get; set; }
        public int Number {  get; set; }
        public float Price { get; set; }
        public float Total {  get; set; }
    }
}
