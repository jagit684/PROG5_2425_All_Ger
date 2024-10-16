using MyDomain;
using System.Collections.Generic;

namespace CarsWebApp.ViewModels
{
    public class CarsViewModel
    {
        public required List<Car> Cars { get; set; }
        public required List<CarColor> CarColors { get; set; }


    }
}
