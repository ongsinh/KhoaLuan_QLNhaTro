namespace KhoaLuan_QLNhaTro.Models
{
    public class Room
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Status { get; set; }
        public int Number { get; set; }
        public float Acreage { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public Guid HouseId { get; set; }
        public Guid AccountId { get; set; }
    }
}
