using MyDomain;

namespace CarsWebApp.Models
{
    public class CarVM
    {
        public Car car { get; set; }
        public CarColor carColor { get; set; }
        public string carColorName { get; set; } = string.Empty;


    }
}
