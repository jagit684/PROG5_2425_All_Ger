using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MyDomain;
public class Car
{
    [Key]
    [Range(1, int.MaxValue)]
    public int Id { get; set; }

    [Required]
    [StringLength(15)]
    public string Brand { get; set; } = String.Empty;

    [Required]
    [StringLength(25)]
    public string Type { get; set; } = String.Empty;

    [Range(10000, 99999)]
    [Required]
    public int Price { get; set; } = 25000;

    [Range(2000, 2024)]
    public int Year { get; set; } = 2022;

    // Foreign key property
    [ForeignKey("CarColor")]
    [AllowNull]
    public int? CarColorId { get; set; }

    // Navigation property
    [AllowNull]
    public CarColor? CarColor { get; set; }

    public ICollection<CarHasOption> CarHasOptions { get; set; } = new List<CarHasOption>();


    public Car()
    {
    }

    public Car(int id, string brand, string type, int price, int? colorId)
    {
        this.Id = id;
        this.Brand = brand;
        this.Type = type;
        this.Price = price;
        this.CarColorId = colorId;
    }
}
