using MyDomain;
using System.ComponentModel.DataAnnotations;


namespace CarsWebApp.ViewModels
{
    public class CarVM
    {

        public Car Car { get; set; }


        public int CalculatedPrice
        {
            get
            {
                int total = Car.Price;
                foreach (var option in AllCarOptions)
                {
                    if (option.IsSelected)
                    {
                        total += option.Price;
                    }
                }
                return total;
            }
        }

        public virtual List<CarColor> AllCarColors { get; set; }

        public List<CarOptionVM> AllCarOptions { get; set; } = new List<CarOptionVM>();

        public List<int> SelectedCarOptions { get; set; }

        public CarVM()
        {
            Car = new Car();
            AllCarColors = new List<CarColor>();
            AllCarOptions = new List<CarOptionVM>();
            SelectedCarOptions = new List<int>();
        }
        public CarVM(Car newCar, List<CarColor> allCarColors, 
            List<int> selectedCarOptions, List<CarOption> allCarOptions)
        {
            Car = newCar;
            AllCarColors = allCarColors;
            SelectedCarOptions = selectedCarOptions ?? new List<int>();
            InitOptions(selectedCarOptions, allCarOptions);
        }


        private void InitOptions(List<int>? selectedCarOptions, List<CarOption> allCarOptions)
        {
            if (selectedCarOptions != null)
            {
                SelectedCarOptions = selectedCarOptions;
                AllCarOptions = new List<CarOptionVM>();
                foreach (var option in allCarOptions)
                {
                    AllCarOptions.Add(new CarOptionVM(
                        option.Id,
                        option.Option,
                        option.Price,
                        selectedCarOptions.Contains(option.Id)));
                }
            }
            else
            {
                AllCarOptions = new List<CarOptionVM>();
                foreach (var option in allCarOptions)
                {
                    AllCarOptions.Add(new CarOptionVM(
                        option.Id,
                        option.Option,
                        option.Price,
                        false));
                }
            }
        }
    }

    public class CarOptionVM
    {
        public int Id { get; set; }
        public string Option { get; set; } = string.Empty;
        public int Price { get; set; }
        public bool IsSelected { get; set; }

        public CarOptionVM(int id, string option, int price, bool isSelected)
        {
            Id = id;
            Option = option;
            Price = price;
            IsSelected = isSelected;
        }
    }
}

