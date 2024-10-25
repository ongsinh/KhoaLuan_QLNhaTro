using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KhoaLuan_QLNhaTro.Models
{
    public class DetailContract
    {
        public string idContract { get; set; }
        public Guid idAsset { get; set; }
        public Guid idService { get; set; }
        public int Number {  get; set; }
        public float Price { get; set; }
        [ForeignKey("idContract"), Required]
        public Contract Contract { get; set; }
        [ForeignKey("idAsset"), Required]
        public Asset Asset { get; set; }
        [ForeignKey("idService"), Required]
        public Service Service { get; set; }
    }
}
