using System.Text.Json.Serialization;

namespace KhoaLuan_QLNhaTro.Models.ViewModel
{
    public class BillDataModel
    {
        [JsonPropertyName("roomId")]
        public Guid RoomId { get; set; }
        [JsonPropertyName("billÌnfor ")]
        public BillInfor billÌnfor { get; set; }
        [JsonPropertyName("services")]
        public List<ServiceModel> services { get; set; }
    }
}
