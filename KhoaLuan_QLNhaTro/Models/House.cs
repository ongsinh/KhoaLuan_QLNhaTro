namespace KhoaLuan_QLNhaTro.Models
{
    public class House
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int floorNumber { get; set; }
        public int roomNumber { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
