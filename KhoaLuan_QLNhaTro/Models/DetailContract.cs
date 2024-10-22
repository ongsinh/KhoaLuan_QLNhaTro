namespace KhoaLuan_QLNhaTro.Models
{
    public class DetailContract
    {
        public string ContractId { get; set; }
        public Guid AssetId { get; set; }
        public Guid ServiceId { get; set; }
        public int Number {  get; set; }
        public float Price { get; set; }
    }
}
