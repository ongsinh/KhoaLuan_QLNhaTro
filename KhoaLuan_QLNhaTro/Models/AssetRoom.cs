using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KhoaLuan_QLNhaTro.Models
{
    public class AssetRoom
    {
        public Guid idRoom { get; set; }
        public Guid idAsset { get; set; }
        public int Number {  get; set; }
        public float Price { get; set; }
        [ForeignKey("idRoom"), Required]
        public Room Room { get; set; }
        [ForeignKey("idAsset"), Required]
        public Asset Asset { get; set; }
       
    }
}
