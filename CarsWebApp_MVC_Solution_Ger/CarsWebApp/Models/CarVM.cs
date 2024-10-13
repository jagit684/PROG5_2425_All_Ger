using Microsoft.EntityFrameworkCore;
using MyDomain;
using System.ComponentModel.DataAnnotations;

namespace CarsWebApp.Models
{
    public class CarVM
    {
        [Key]
        public int Id { get; set; }
        public virtual Car Car { get; set; }
        public virtual List<CarColor> CarColors { get; set; }
        public int? CarColorId { get; set; }

        public CarVM() {
            Car = new Car();
            CarColors = new List<CarColor>();
        }

        public CarVM(int Id, Car newCar, List<CarColor> carColors)
        {
            this.Id = Id;
            Car = newCar;
            CarColors = carColors;
        }

    }
}
