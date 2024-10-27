using System.ComponentModel.DataAnnotations;

namespace MyDomain
{
    public class CarOption
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Option { get; set; } = String.Empty;
        [Range(100, 10000)]
        public int Price { get; set; }

        public ICollection<CarHasOption> CarHasOptions { get; set; } = new List<CarHasOption>();
    }
}
