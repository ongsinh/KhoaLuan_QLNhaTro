using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace KhoaLuan_QLNhaTro.Models
{
    [PrimaryKey(nameof(AssetId), nameof(RoomId))]
    public class AssetRoom
    {
        public Guid RoomId { get; set; }
        public Guid AssetId { get; set; }
        public int Number {  get; set; }
        public float Price { get; set; }
        public virtual Asset Asset { get; set; }
        public virtual Room Room { get; set; }
    }
}
