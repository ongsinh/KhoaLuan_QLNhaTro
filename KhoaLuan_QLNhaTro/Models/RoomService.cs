using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace KhoaLuan_QLNhaTro.Models
{
    [PrimaryKey(nameof(RoomId), nameof(ServiceId))]
    public class RoomService
    {
        public Guid RoomId { get; set; }
        public Guid ServiceId { get; set; }
        public int Number {  get; set; }
        public decimal Price { get; set; }
        public virtual Room Room { get; set; }
        public virtual Service Service { get; set; }
    }
}
