using Microsoft.EntityFrameworkCore;
using MyDomain;
using System.ComponentModel.DataAnnotations;

namespace CarsWebApp.Models
{
    public class CarVM
    {
        public virtual Car Car { get; set; }
        public virtual List<CarColor> CarColors { get; set; }

        public CarVM() {
            Car = new Car();
            CarColors = new List<CarColor>();
        }

        public CarVM(Car newCar, List<CarColor> carColors)
        {
            Car = newCar;
            CarColors = carColors;
        }

    }
}
