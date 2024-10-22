namespace KhoaLuan_QLNhaTro.Models
{
    public class Contract
    {
        public string Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set;}
        public DateTime Time {  get; set; }
        public string Status { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public Guid RoomId { get; set; }
        public Guid AccountId { get; set; }
    }
}
