using Microsoft.EntityFrameworkCore;
using MyDomain;

namespace CarsWebApp.DbAcces;

public partial class MyCarsDbContext : DbContext
{
    public MyCarsDbContext()
    {
    }

    public MyCarsDbContext(DbContextOptions<MyCarsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Car> Cars { get; set; }

    public DbSet<CarColor> CarColors { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>()
                .HasOne(c => c.CarColor)
                .WithMany(cc => cc.Cars)
                .HasForeignKey(c => c.CarColorId)
                .IsRequired(false);

        modelBuilder.Entity<Car>()
               .Property(c => c.Id)
               .ValueGeneratedNever(); // This allows explicit values to be set for the Id property

        modelBuilder.Entity<CarColor>().HasData(
            new CarColor { Id = 1, Color = "Red" },
            new CarColor { Id = 2, Color = "Blue" },
            new CarColor { Id = 3, Color = "Black" },
            new CarColor { Id = 4, Color = "Gold" },
            new CarColor { Id = 5, Color = "Silver" }
        );

        modelBuilder.Entity<Car>().HasData(
            new Car { Id = 1, Brand = "Chevrolet", Type = "Camaro", Price = 50000, Year = 2022, CarColorId = 5 },
            new Car { Id = 2, Brand = "Hyundai", Type = "Elantra", Price = 20000, Year = 2022, CarColorId = 3 },
            new Car { Id = 3, Brand = "Volkswagen", Type = "Passat", Price = 30000, Year = 2022, CarColorId = 3 },
            new Car { Id = 4, Brand = "Chevrolet", Type = "Equinox", Price = 28000, Year = 2022, CarColorId = 4 },
            new Car { Id = 5, Brand = "Volkswagen", Type = "Jetta", Price = 27000, Year = 2022, CarColorId = 2 }
        );

    }

}
