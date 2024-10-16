using MyDomain;
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

        public virtual List<CarColor> AllCarColors { get; set; }

        // List of all car options with their selected status
        public List<CarOptionVM> AllCarOptions { get; set; } = new List<CarOptionVM>();

        public List<int> SelectedCarOptions { get; set; }

        public CarVM()
        {
            AllCarColors = new List<CarColor>();
            AllCarOptions = new List<CarOptionVM>();
            SelectedCarOptions = new List<int>();
        }
        public CarVM(int Id, List<CarColor> allCarColors, List<int> selectedCarOptions, List<CarOption> allCarOptions)
        {
            this.Id = Id;
            this.AllCarColors = allCarColors;
            SelectedCarOptions = selectedCarOptions ?? new List<int>();
            InitOptions(selectedCarOptions, allCarOptions);
        }


        public CarVM(int id, string brand, string type, int price, int year, int colorId,
                        List<CarColor> allCarColors,
                        List<int> selectedCarOptions, List<CarOption> allCarOptions)
        {
            this.Id = id;
            this.Brand = brand;
            this.Type = type;
            this.Price = price;
            this.Year = year;
            this.AllCarColors = allCarColors;
            InitColor(colorId);
            SelectedCarOptions = selectedCarOptions ?? new List<int>();
            InitOptions(selectedCarOptions, allCarOptions);
        }

        private void InitColor(int colorId)
        {
            this.CarColorId = colorId;
            this.CarColor = AllCarColors?.FirstOrDefault(c => c.Id == colorId);
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
                        false));
                }
            }
        }
    }

    public class CarOptionVM
    {
        public int Id { get; set; }
        public string Option { get; set; } = string.Empty;
        public bool IsSelected { get; set; }

        public CarOptionVM(int id, string option, bool isSelected)
        {
            Id = id;
            Option = option;
            IsSelected = isSelected;
        }
    }


}

