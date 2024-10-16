using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
