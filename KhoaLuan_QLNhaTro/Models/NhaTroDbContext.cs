using Microsoft.EntityFrameworkCore;

namespace KhoaLuan_QLNhaTro.Models
{
    public class NhaTroDbContext : DbContext
    {
        public NhaTroDbContext(DbContextOptions options) : base(options)
        {
        }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Asset> Assets { get; set; }
        public virtual DbSet<AssetRoom> AssetRooms { get; set; }
        public virtual DbSet<Bill> Bills { get; set; }
        public virtual DbSet<Contract> Contracts { get; set; }
        public virtual DbSet<DetailBill> Details { get; set; }
        public virtual DbSet<DetailContract> DetailContracts { get; set; }
        public virtual DbSet<House> Houses { get; set; }
        public virtual DbSet<Incident> Incidents { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomService> RoomsServices { get; set;}
        public virtual DbSet<Service> Services { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Bill>()
                .HasOne(b => b.Room)
                .WithMany(r => r.Bills)
                .HasForeignKey(b => b.RoomId)
                .OnDelete(DeleteBehavior.NoAction); // Thay đổi thành NoAction hoặc Restrict

            modelBuilder.Entity<Bill>()
                .HasOne(b => b.Account)
                .WithMany(a => a.Bills)
                .HasForeignKey(b => b.AccountId)
                .OnDelete(DeleteBehavior.Cascade); // Có thể giữ Cascade cho mối quan hệ này

            modelBuilder.Entity<Contract>()
        .HasOne(c => c.Room)
        .WithOne(r => r.Contract) // Định nghĩa mối quan hệ 1-1
        .HasForeignKey<Contract>(c => c.RoomId) // Khóa ngoại trong Contract
        .OnDelete(DeleteBehavior.NoAction);
        }

    }
}
