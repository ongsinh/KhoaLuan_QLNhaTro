﻿namespace KhoaLuan_QLNhaTro.Models
{
    public class Bill
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public float Total { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public Guid RoomId { get; set; }
        public Guid AccountId { get; set; }
    }
}
