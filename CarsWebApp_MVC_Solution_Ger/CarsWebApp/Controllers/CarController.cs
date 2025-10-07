using CarsWebApp.DbAccess;
using CarsWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyDomain;

namespace CarsWebApp_Start.Controllers
{
    public class CarController : Controller
    {
        private readonly ILogger<CarController> logger;

        private MyCarsDbContext dbContext;

        public CarController(MyCarsDbContext pdbContext, ILogger<CarController> logger)
        {
            this.dbContext = pdbContext;
            this.logger = logger;
        }

        [HttpGet]
        public ActionResult Index()
        {
            List<Car> carlist = dbContext.Cars
                    .OrderBy(c => c.Brand)
                    .ThenBy(c => c.Type)
                    .Include(c => c.CarColor)
                    .Include(c => c.CarHasOptions)
                    .ToList();

            List<CarVM> carVMlist = new List<CarVM>();

            var allCarsColors = dbContext.CarColors.ToList();
            var allCarOptions = dbContext.CarOptions.ToList();


            foreach (Car car in carlist)
            {
                var selectedCarOptions = car.CarHasOptions
                                       .Where(o => o.CarId == car.Id)
                                       .Select(o => o.CarOptionId)
                                       .ToList();
                CarVM carVM = new CarVM(car, allCarsColors, selectedCarOptions, allCarOptions);
                carVMlist.Add(carVM);
            }

            return View(carVMlist);
        }


        [HttpGet]
        public ActionResult Create()
        {
            int newId;
            try
            {
                newId = dbContext.Cars.Max(c => c.Id) + 1;
            }
            catch
            {
                newId = 1;
            }

            Car newCar = new Car();
            newCar.Id = newId;
            var allCarColors = dbContext.CarColors.ToList();
            var AllCarOptions = dbContext.CarOptions.ToList();
            var selectedCarOptions = new List<int>();

            CarVM carVM = new CarVM(newCar, allCarColors, selectedCarOptions, AllCarOptions);
            return View(carVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CarVM carVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Car newcar = new Car(carVM.Car.Id, carVM.Car.Brand, carVM.Car.Type, carVM.Car.Price, carVM.Car.Year, carVM.Car.CarColorId);
                    dbContext.Cars.Add(newcar);

                    foreach (var optionId in carVM.SelectedCarOptions)
                    {
                        newcar.CarHasOptions.Add(new CarHasOption
                        {
                            CarId = newcar.Id,
                            CarOptionId = optionId
                        });
                    }

                    await dbContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error creating car with id: {Id}", carVM.Car.Id);
                }
            }
            else
            {
                logger.LogError("ModelState is Invalid. Car ID: {Id}", carVM.Car.Id);
            }

            carVM.AllCarColors = dbContext.CarColors.ToList();
            return View(carVM);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDelete = await dbContext.Cars
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toDelete == null)
            {
                return NotFound();
            }

            return View(toDelete);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var toDelete = await dbContext.Cars.FindAsync(id);
            if (toDelete != null)
            {
                dbContext.Cars.Remove(toDelete);
            }

            await dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            Car? currentCar = dbContext.Cars.Include(c => c.CarColor).FirstOrDefault(c => c.Id == id);

            if (currentCar != null)
            {
                return View(currentCar);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public ActionResult EditCarWithOptions(int id)
        {
            Car? currentCar = dbContext.Cars
                .Include(c => c.CarHasOptions)
                .FirstOrDefault(c => c.Id == id);
            if (currentCar != null)
            {
                var allCarColors = dbContext.CarColors.ToList();
                var AllCarOptions = dbContext.CarOptions.ToList();
                var selectedCarOptions = dbContext.CarHasOptions
                                            .Where(o => o.CarId == id)
                                            .Select(o => o.CarOptionId)
                                            .ToList();

                CarVM carVM = new CarVM(currentCar, allCarColors, selectedCarOptions, AllCarOptions);
                return View(carVM);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCarWithOptions(CarVM currentCar)
        {
            if (ModelState.IsValid)
            {
                Car? carToUpdate = dbContext.Cars
                    .Include(c => c.CarHasOptions)
                    .FirstOrDefault(c => c.Id == currentCar.Car.Id);
                if (carToUpdate != null)
                {

                    // Update car properties
                    dbContext.Entry(carToUpdate).CurrentValues.SetValues(currentCar.Car);

                    // Update CarHasOptions
                    carToUpdate.CarHasOptions.Clear();
                    foreach (var optionId in currentCar.SelectedCarOptions)
                    {
                        carToUpdate.CarHasOptions.Add(new CarHasOption
                        {
                            CarId = carToUpdate.Id,
                            CarOptionId = optionId
                        });
                    }
                    dbContext.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
            }
            currentCar.AllCarColors = dbContext.CarColors.ToList();
            return View(currentCar);
        }

        public ActionResult Edit(int id)
        {
            Car? currentCar = dbContext.Cars.FirstOrDefault(c => c.Id == id);
            if (currentCar != null)
            {
                IEnumerable<CarColor> carColors = dbContext.CarColors
                               .OrderBy(c => c.Color);
                ViewBag.CarColors = carColors;

                return View(currentCar);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Car currentCar)
        {
            if (ModelState.IsValid)
            {
                Car? carToUpdate = dbContext.Cars.FirstOrDefault(c => c.Id == currentCar.Id);
                if (carToUpdate != null)
                {
                    dbContext.Entry(carToUpdate).CurrentValues.SetValues(currentCar);
                    dbContext.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View();
                }
            }
            return View(currentCar);
        }


        [HttpGet]
        public ActionResult ListCarsAndColors()

        {
            var cars = dbContext.Cars
                        .Include(c => c.CarColor)
                        .OrderBy(c => c.Brand)
                        .ThenBy(c => c.Type)
                        .ToList();
            ViewBag.CarColors = dbContext.CarColors
                                    .OrderBy(c => c.Color)
                                    .ToList();
            return View(cars);
        }



        [HttpPost]
        public ActionResult UpdateCarColor(Car submittedCar)
        {
            logger.LogInformation("UpdateCarColor called with id: {Id}, carColorId: {CarColorId}", submittedCar.Id, submittedCar.CarColorId);

            var foundCar = dbContext.Cars.Find(submittedCar.Id);
            if (foundCar == null)
            {
                logger.LogError("Car with id: {Id} not found and carColorId: {CarColorId}", submittedCar.Id, submittedCar.CarColorId);
            }
            else
            {
                foundCar.CarColorId = submittedCar.CarColorId;
                dbContext.SaveChanges();

                logger.LogInformation("Car with id: {Id} updated with new carColorId: {CarColorId}", submittedCar.Id, submittedCar.CarColorId);
            }
            return RedirectToAction(nameof(ListCarsAndColors));
        }
    }
}


