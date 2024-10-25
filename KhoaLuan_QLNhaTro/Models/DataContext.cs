using Microsoft.EntityFrameworkCore;

namespace KhoaLuan_QLNhaTro.Models
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<House> Houses { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<AssetRoom> AssetRooms { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<DetailBill> DetailBills { get; set; }
        public DbSet<DetailContract> DetailContracts { get; set; }
        public DbSet<RoomService> RoomServices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                .UseLoggerFactory(loggerFactory);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AssetRoom>()
                .HasKey(ar => new { ar.idRoom, ar.idAsset });
            modelBuilder.Entity<DetailBill>()
                .HasKey(ar => new { ar.idBill, ar.idService });
            modelBuilder.Entity<DetailContract>()
                .HasKey(ar => new { ar.idContract, ar.idService ,ar.idAsset});
            modelBuilder.Entity<RoomService>()
                .HasKey(ar => new { ar.idRoom, ar.idService });


            // Thiết lập quan hệ khóa ngoại cho Bill với Room, chỉ định hành vi OnDelete
            modelBuilder.Entity<Bill>()
                .HasOne(b => b.Room)
                .WithMany(r => r.Bills) // Đảm bảo Room có thuộc tính Bills nếu không sẽ gây lỗi
                .HasForeignKey(b => b.idRoom)
                .OnDelete(DeleteBehavior.Restrict); // Hoặc DeleteBehavior.NoAction

            // Thiết lập quan hệ khóa ngoại cho Bill với Account, chỉ định hành vi OnDelete
            modelBuilder.Entity<Bill>()
                .HasOne(b => b.Account)
                .WithMany()
                .HasForeignKey(b => b.idAccount)
                .OnDelete(DeleteBehavior.Restrict); // Hoặc DeleteBehavior.NoAction
            modelBuilder.Entity<Contract>()
                .HasOne(b => b.Room)
                .WithMany(r => r.Contracts) // Đảm bảo Room có thuộc tính Bills nếu không sẽ gây lỗi
                .HasForeignKey(b => b.idRoom)
                .OnDelete(DeleteBehavior.Restrict); // Hoặc DeleteBehavior.NoAction

            // Thiết lập quan hệ khóa ngoại cho Bill với Account, chỉ định hành vi OnDelete
            modelBuilder.Entity<Contract>()
                .HasOne(b => b.Account)
                .WithMany()
                .HasForeignKey(b => b.idAccount)
                .OnDelete(DeleteBehavior.Restrict); // Hoặc DeleteBehavior.NoAction


            base.OnModelCreating(modelBuilder);
        }
    }
}
