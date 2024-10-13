using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyDomain
{
    public class CarColor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }


        private string _color;
        [Required]
        [StringLength(15)]
        public string Color
        {
            get { return _color; }
            set { _color = CapitalizeFirstLetter(value ?? string.Empty); }
        }

        public ICollection<Car> Cars { get; set; }

        public CarColor()
        {
            Cars = new List<Car>();
            _color = string.Empty; // Initialize _color to avoid CS8618
            Color = string.Empty;
        }

        public CarColor(int id, string color)
        {
            this.Id = id;
            this._color = string.Empty; // Initialize _color to avoid CS8618
            this.Color = color;
            Cars = new List<Car>();
        }

        private string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            else
            {
                return char.ToUpper(input[0]) + input.Substring(1);
            }
        }
    }
}
