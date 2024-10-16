using CarsWebApp.DbAcces;
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
            IEnumerable<Car> carlist = dbContext.Cars
                    .OrderBy(c => c.Brand)
                    .ThenBy(c => c.Type)
                    .Include(c => c.CarColor);

            return View(carlist);
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

            var allCarColors = dbContext.CarColors.ToList();
            var AllCarOptions = dbContext.CarOptions.ToList();
            var selectedCarOptions = new List<int>();

            CarVM carVM = new CarVM(newId, allCarColors, selectedCarOptions, AllCarOptions);
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
                    Car car = new Car(carVM.Id, carVM.Brand, carVM.Type, carVM.Price, carVM.Year, carVM.CarColorId);
                    dbContext.Cars.Add(car);
                    await dbContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error creating car with id: {Id}", carVM.Id);
                }
            }
            else
            {
                logger.LogError("ModelState is Invalid. Car ID: {Id}", carVM.Id);
            }

            carVM.AllCarColors = dbContext.CarColors.ToList();
            return View(carVM);
        }


        // GET: CarColors/Delete/5
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

        // POST: CarColors/Delete/5
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

        public ActionResult Edit(int id)
        {
            Car? currentCar = dbContext.Cars.FirstOrDefault(c => c.Id == id);
            if (currentCar != null)
            {
                //IEnumerable<SelectListItem> carColors = dbContext.CarColors
                //              .OrderBy(c => c.Color)
                //              .Select(c => new SelectListItem
                //              {
                //                  Value = c.Id.ToString(),
                //                  Text = c.Color
                //              }).ToList();
                //ViewBag.CarColors = carColors;

                IEnumerable<CarColor> carColors = dbContext.CarColors
                               .OrderBy(c => c.Color);
                               //.ToList();
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
            var cars = dbContext.Cars.Include(c => c.CarColor).ToList();
            ViewBag.CarColors = dbContext.CarColors.ToList();

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


