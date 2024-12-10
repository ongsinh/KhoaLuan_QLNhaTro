using Microsoft.EntityFrameworkCore;
using VnPayIntegration.Models;

namespace KhoaLuan_QLNhaTro.Models
{
    public class NhaTroDbContext : DbContext
    {
        public NhaTroDbContext(DbContextOptions options) : base(options)
        {
        }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Asset> Assets { get; set; }
        public virtual DbSet<IncidentRoom> IncidentRooms { get; set; }
        public virtual DbSet<Bill> Bills { get; set; }
        public virtual DbSet<Contract> Contracts { get; set; }
        public virtual DbSet<DetailBill> DetailBills { get; set; }
        public virtual DbSet<DetailContract> DetailContracts { get; set; }
        public virtual DbSet<House> Houses { get; set; }
        public virtual DbSet<Incident> Incidents { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomService> RoomsServices { get; set;}
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<PaymentResponseModel> PaymentResponseModels { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Bill>()
                .HasOne(b => b.Room)
                .WithMany(r => r.Bills)
                .HasForeignKey(b => b.RoomId)
                .OnDelete(DeleteBehavior.NoAction); // Thay đổi thành NoAction hoặc Restrict

            modelBuilder.Entity<Bill>()
                .HasOne(b => b.User)
                .WithMany(a => a.Bills)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Có thể giữ Cascade cho mối quan hệ này

            modelBuilder.Entity<Contract>()
                .HasOne(c => c.Room)
                .WithOne(r => r.Contract) // Định nghĩa mối quan hệ 1-1
                .HasForeignKey<Contract>(c => c.RoomId) // Khóa ngoại trong Contract
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<IncidentRoom>()
                .HasOne(ir => ir.Room)
                .WithMany()  // Không cần ICollection trong Room
                .HasForeignKey(ir => ir.RoomId)
                .OnDelete(DeleteBehavior.Restrict); // Hoặc DeleteBehavior.NoAction

            modelBuilder.Entity<IncidentRoom>()
                .HasOne(ir => ir.Incident)
                .WithMany()  // Không cần ICollection trong Incident
                .HasForeignKey(ir => ir.IncidentId)
                .OnDelete(DeleteBehavior.Restrict); // Hoặc DeleteBehavior.NoAction

            modelBuilder.Entity<DetailContract>()
                .HasOne(ir => ir.Asset)
                .WithMany()  // Không cần ICollection trong Room
                .HasForeignKey(ir => ir.AssetId)
                .OnDelete(DeleteBehavior.Restrict); // Hoặc DeleteBehavior.NoAction

            modelBuilder.Entity<DetailContract>()
                .HasOne(ir => ir.Contract)
                .WithMany()  // Không cần ICollection trong Incident
                .HasForeignKey(ir => ir.ContractId)
                .OnDelete(DeleteBehavior.Restrict); // Hoặc DeleteBehavior.NoAction
            modelBuilder.Entity<DetailBill>()
                .HasOne(c => c.Service)
                .WithMany(d => d.DetailBills)
                .HasForeignKey(c => c.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
