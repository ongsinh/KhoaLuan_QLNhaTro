using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace KhoaLuan_QLNhaTro.Models
{
    [PrimaryKey(nameof(IncidentId), nameof(RoomId))]
    public class IncidentRoom
    {
        public Guid RoomId { get; set; }
        public Guid IncidentId { get; set; }
        public string Status {  get; set; }
        public virtual Incident Incident { get; set; }
        public virtual Room Room { get; set; }
    }
}
