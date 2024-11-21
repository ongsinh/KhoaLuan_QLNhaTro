using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KhoaLuan_QLNhaTro.Models
{
    public class Room
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Status { get; set; }
        public int FLoorNumber { get; set; }
        public float Acreage { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public Guid HouseId { get; set; }
        public Guid UserId { get; set; }
        public virtual House House { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Bill> Bills { get; set; }
        public virtual Contract Contract { get; set; }
        public virtual ICollection<Asset> Assets { get; set; }
        public virtual ICollection<RoomService> RoomServices { get; set; }
    }
}
