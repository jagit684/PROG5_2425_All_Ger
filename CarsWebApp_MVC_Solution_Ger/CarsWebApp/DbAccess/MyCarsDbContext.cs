using Microsoft.EntityFrameworkCore;
using MyDomain;
using CarsWebApp.ViewModels;

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
    public DbSet<CarOption> CarOptions { get; set; }
    public DbSet<CarHasOption> CarHasOptions { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Car>()
                .HasOne(c => c.CarColor)
                .WithMany(cc => cc.Cars)
                .HasForeignKey(c => c.CarColorId)
                .IsRequired(false);

        modelBuilder.Entity<Car>()
               .Property(c => c.Id)
               .ValueGeneratedNever(); // This allows explicit values to be set for the Id property

        // Configure the many-to-many relationship
        modelBuilder.Entity<CarHasOption>()
            .HasKey(co => new { co.CarId, co.CarOptionId });

        modelBuilder.Entity<CarHasOption>()
            .HasOne(co => co.Car)
            .WithMany(c => c.CarHasOptions)
            .HasForeignKey(co => co.CarId);

        modelBuilder.Entity<CarHasOption>()
            .HasOne(co => co.CarOption)
            .WithMany(o => o.CarHasOptions)
            .HasForeignKey(co => co.CarOptionId);

        modelBuilder.Entity<CarColor>().HasData(
            new CarColor { Id = 1, Color = "Red" },
            new CarColor { Id = 2, Color = "Blue" },
            new CarColor { Id = 3, Color = "Black" },
            new CarColor { Id = 4, Color = "Gold" },
            new CarColor { Id = 5, Color = "Silver" }
        );

        modelBuilder.Entity<Car>().HasData(
           new Car { Id = 1, Brand = "Chevrolet", Type = "Camaro", Price = 38000, Year = 2022, CarColorId = 5 }, // Average price[^1^](https://www.cargurus.com/research/2022-Chevrolet-Camaro-c31446)
           new Car { Id = 2, Brand = "Hyundai", Type = "Elantra", Price = 19000, Year = 2022, CarColorId = 3 }, // Average price[^2^](https://cars.usnews.com/cars-trucks/hyundai/elantra/2022)
           new Car { Id = 3, Brand = "Volkswagen", Type = "Passat", Price = 21500, Year = 2022, CarColorId = 3 }, // Average price[^3^](https://cars.usnews.com/cars-trucks/volkswagen/passat)
           new Car { Id = 4, Brand = "Chevrolet", Type = "Equinox", Price = 21000, Year = 2022, CarColorId = 4 }, // Average price[^4^](https://cars.usnews.com/cars-trucks/chevrolet/equinox/2022)
           new Car { Id = 5, Brand = "Volkswagen", Type = "Jetta", Price = 19500, Year = 2022, CarColorId = 2 }, // Average price[^5^](https://cars.usnews.com/cars-trucks/volkswagen/jetta/2022)
           new Car { Id = 6, Brand = "Chevrolet", Type = "Malibu", Price = 20000, Year = 2022, CarColorId = 1 }, // Average price[^6^](https://cars.usnews.com/cars-trucks/chevrolet/malibu/2022)
           new Car { Id = 7, Brand = "Hyundai", Type = "Santa Fe", Price = 24000, Year = 2022, CarColorId = 4 }, // Average price[^7^](https://cars.usnews.com/cars-trucks/hyundai/santa-fe/2022)
           new Car { Id = 8, Brand = "Volkswagen", Type = "Tiguan", Price = 21600, Year = 2022, CarColorId = 5 }, // Average price[^8^](https://cars.usnews.com/cars-trucks/volkswagen/tiguan/2022)
           new Car { Id = 9, Brand = "Chevrolet", Type = "Trailblazer", Price = 20000, Year = 2022, CarColorId = 3 } // Average price[^9^](https://cars.usnews.com/cars-trucks/chevrolet/trailblazer/2022)
       );

        modelBuilder.Entity<CarOption>().HasData(
            new CarOption { Id = 1, Option = "Leather Seats", Price = 1500 },
            new CarOption { Id = 2, Option = "Sunroof", Price = 1200 },
            new CarOption { Id = 3, Option = "Navigation System", Price = 1000 },
            new CarOption { Id = 4, Option = "Heated Seats", Price = 500 },
            new CarOption { Id = 5, Option = "Bluetooth", Price = 300 },
            new CarOption { Id = 6, Option = "Backup Camera", Price = 200 },
            new CarOption { Id = 7, Option = "Remote Start", Price = 400 },
            new CarOption { Id = 8, Option = "Blind Spot Monitoring", Price = 600 },
            new CarOption { Id = 9, Option = "Parking Sensors", Price = 400 },
            new CarOption { Id = 10, Option = "Lane Departure Warning", Price = 1500 },
            new CarOption { Id = 11, Option = "Adaptive Cruise Control", Price = 1500 },
            new CarOption { Id = 12, Option = "Apple CarPlay", Price = 300 },
            new CarOption { Id = 13, Option = "Android Auto", Price = 300 },
            new CarOption { Id = 14, Option = "Wi-Fi Hotspot", Price = 200 },
            new CarOption { Id = 15, Option = "Keyless Entry", Price = 300 },
            new CarOption { Id = 16, Option = "Power Liftgate", Price = 500 },
            new CarOption { Id = 17, Option = "Satellite Radio", Price = 300 },
            new CarOption { Id = 18, Option = "Automatic Parking", Price = 1000 },
            new CarOption { Id = 19, Option = "Surround-View Camera", Price = 800 },
            new CarOption { Id = 20, Option = "Head-Up Display", Price = 1000 }
        );

    }

public DbSet<CarsWebApp.ViewModels.CarVM> CarVM { get; set; } = default!;

}
