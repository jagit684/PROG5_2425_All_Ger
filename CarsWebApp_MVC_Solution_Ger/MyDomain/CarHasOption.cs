using System.ComponentModel.DataAnnotations;

namespace MyDomain
{
    public class CarHasOption
    {
        [Key]
        public int CarId { get; set; }

        [Required]
        public Car Car { get; set; } = null!;

        [Key]
        public int CarOptionId { get; set; }

        [Required]
        public CarOption CarOption { get; set; } = null!;
    }
}
