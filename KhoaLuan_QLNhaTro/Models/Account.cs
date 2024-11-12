using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KhoaLuan_QLNhaTro.Models
{
    public class Account
    {
        [Key]
        public Guid Id { get; set; }
        public float Name { get; set; }
        public string CCCD { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime Dob { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set;}
        public Guid RoleId { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<Bill> Bills { get; set; }
    }
}
