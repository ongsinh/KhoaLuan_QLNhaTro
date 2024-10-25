using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KhoaLuan_QLNhaTro.Models
{
    public class RoomService
    {
        public Guid idRoom { get; set; }
        public Guid idService { get; set; }
        public int Number {  get; set; }
        public float Price { get; set; }
        [ForeignKey("idRoom"), Required]
        public Room Room { get; set; }
        [ForeignKey("idService"), Required]
        public Service Service { get; set; }
    }
}
