//using Microsoft.EntityFrameworkCore;
//using MyDomain;
//using System.ComponentModel.DataAnnotations;

//namespace CarsWebApp.Models
//{
//    public class CarVM
//    {
//        public virtual Car Car { get; set; }
//        public virtual List<CarColor> CarColors { get; set; }

//        public virtual List<CarOption> CasOptions { get; set; }

//        public CarVM() {
//            Car = new Car();
//            CarColors = new List<CarColor>();
//        }

//        public CarVM(Car newCar, List<CarColor> carColors)
//        {
//            Car = newCar;
//            CarColors = carColors;
//        }

//    }
//}

using MyDomain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarsWebApp.ViewModels
{
    public class CarVM
    {
        public int Id { get; set; }

        [Required]
        [StringLength(15)]
        public string Brand { get; set; } = string.Empty;

        [Required]
        [StringLength(25)]
        public string Type { get; set; } = string.Empty;

        [Range(10000, 99999)]
        [Required]
        public int Price { get; set; } = 25000;

        [Range(2000, 2024)]
        public int Year { get; set; } = 2022;

        public int? CarColorId { get; set; }
        public CarColor? CarColor { get; set; }

        public virtual List<CarColor> CarColors { get; set; }

        // List of all car options with their selected status
        public List<CarOptionVM> CarOptions { get; set; } = new List<CarOptionVM>();

        public CarVM()
        {
            CarColors = new List<CarColor>();
        }
        public CarVM(int Id, List<CarColor> carColors)
        {
            this.Id = Id;
            CarColors = carColors;
        }

        public CarVM(int id, string brand, string type, int price, int colorId, List<CarColor> carColors)
        {
            this.Id = Id;
            this.Brand = Brand;
            this.Type = Type;
            this.Price = Price;
            this.Year = Year;
            this.CarColorId = CarColorId;
            this.CarColor = CarColor;
            CarColors = carColors;
        }
    }

    public class CarOptionVM
    {
        public int Id { get; set; }
        public string Option { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
    }


}

