using Microsoft.AspNetCore.Mvc;
using PhoneBookWebApp.DbAccess;
using PhoneBookWebApp.ViewModels;
using MyDomain.Models;
using ImageConverter = MyDomain.Models.ImageConverter;

namespace PhoneBookWebApp.Controllers
{
    public class HobbiesController : Controller
    {
        private readonly ImageConverter _imageConverter = new ImageConverter();

        private PhoneBookContext dbContext;

        public HobbiesController(PhoneBookContext pdbContext)
        {
            this.dbContext = pdbContext;
        }

        // GET: HobbiesController
        public ActionResult Index()
        {
                return View(dbContext.Hobbies.OrderBy(h => h.Titel).ToList());
        }

        // GET: HobbiesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HobbiesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HobbiesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HobbyViewModel hobbyVM)
        {
            try
            {
                {
                    Hobby model = hobbyVM.Hobby;
                    if (hobbyVM.picPath != null)
                    {
                        model.HobbyImage = _imageConverter.FilePNGToByteArray(hobbyVM.picPath);
                    }
                    dbContext.Hobbies.Add(model);
                    dbContext.SaveChanges();
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HobbiesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HobbiesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HobbiesController/Delete/5
        public ActionResult Delete(int id)
        {
                Hobby? model = dbContext.Hobbies.Find(id);
                if (model == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                return View(model);
        }

        // POST: HobbiesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            {
                try
                {
                    Hobby? hobby = dbContext.Hobbies.FirstOrDefault(h => h.Id == id);
                    if (hobby != null)
                    {
                        dbContext.Hobbies.Remove(hobby);
                        dbContext.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
        }
    }
}
