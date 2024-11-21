using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KhoaLuan_QLNhaTro.Models
{
    public class Contract
    {
        [Key]
        public string Id { get; set; }
        public DateTime StartDate { get; set; }
        public int Time {  get; set; }
        public string Status { get; set; }
        public int Deposit { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public Guid RoomId { get; set; }
        public Guid UserId { get; set; }
        public virtual Room Room { get; set; }
        public virtual User User { get; set; }
    }
}
