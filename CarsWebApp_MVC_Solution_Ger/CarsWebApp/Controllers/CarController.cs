using CarsWebApp.DbAcces;
using CarsWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyDomain;
using System;

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
        public IActionResult NewCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewCreate([Bind("Brand, Type, Price, Year")] Car car)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    dbContext.Add(car);
                    await dbContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error creating car with id: {Id}", car.Id);

                    return View(car);
                }
            }
            return View(car);
        }





        [HttpGet]
        public ActionResult Create()
        {
            Car newCar = new Car();

            IEnumerable<SelectListItem> carColors = dbContext.CarColors
              .OrderBy(c => c.Color)
              .Select(c => new SelectListItem
              {
                  Value = c.Id.ToString(),
                  Text = c.Color
              }).ToList();
            ViewBag.CarColors = carColors;

            try
            {
                var newId = dbContext.Cars.Max(c => c.Id);
                newCar.Id = newId + 1;
            }
            catch
            {
                newCar.Id = 1;
            }

            return View(newCar);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Car newCar)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    dbContext.Cars.Add(newCar);
                    await dbContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error creating car with id: {Id}", newCar.Id);

                    return View(newCar);
                }
            }
            else
            {
                logger.LogError( "ModelState is Invalid", newCar.Id);
            }
            return View(newCar);
        }


        //[HttpGet]
        //public ActionResult Delete(int? id)
        //{
        //    Car? toDelete = dbContext.Cars.FirstOrDefault(c => c.Id == id);
        //    if (toDelete != null)
        //    {
        //        dbContext.Cars.Remove(toDelete);
        //        dbContext.SaveChanges();
        //    }
        //    return RedirectToAction(nameof(Index));
        //}

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
                IEnumerable<SelectListItem> carColors = dbContext.CarColors
                              .OrderBy(c => c.Color)
                              .Select(c => new SelectListItem
                              {
                                  Value = c.Id.ToString(),
                                  Text = c.Color
                              }).ToList();
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

        //[HttpGet]
        //public ActionResult ListCarsAndColors()
        //{
        //    IEnumerable<Car> carlist = dbContext.Cars
        //            .OrderBy(c => c.Brand)
        //            .ThenBy(c => c.Type)
        //            .Include(c => c.CarColor);

        //    return View(carlist);
        //}


        [HttpGet]
        public ActionResult ListCarsAndColors()

        {
            var cars = dbContext.Cars.Include(c => c.CarColor).ToList();
            var carColors = dbContext.CarColors.ToList();

            var viewModel = new CarsViewModel
            {
                Cars = cars,
                CarColors = carColors
            };

            return View(viewModel);
        }



        [HttpPost]
        public ActionResult UpdateCarColor(int carId, int carColorId)
        //{
        //    var car =  dbContext.Cars.Find(carId);
        //    if (car == null)
        //    {
        //        return NotFound();
        //    }

        //    car.CarColorId = carColorId;
        //    dbContext.SaveChanges();

        //    return RedirectToAction(nameof(ListCarsAndColors));
        //}

        {
            logger.LogInformation("UpdateCarColor called with id: {Id}, carColorId: {CarColorId}", carId, carColorId);

            var car = dbContext.Cars.Find(carId);
            if (car == null)
            {
                logger.LogError("Car with id: {Id} not found and carColorId: {CarColorId}", carId, carColorId);
                return NotFound();
            }

            car.CarColorId = carColorId;
            dbContext.SaveChanges();

            logger.LogInformation("Car with id: {Id} updated with new carColorId: {CarColorId}", carId, carColorId);

            return RedirectToAction(nameof(ListCarsAndColors));
        }
    }
}


