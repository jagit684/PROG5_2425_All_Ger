using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneBookWebApp.DbAccess;
using MyDomain.Models;

namespace PhoneBookWebApp.Controllers
{
    public class PersonsController : Controller
    {

        private PhoneBookContext dbContext;

        public PersonsController(PhoneBookContext pdbContext)
        {
            this.dbContext = pdbContext;
        }



        // GET: PersonsController
        public ActionResult Index()
        {
            ViewBag.postfix = ".png";
            return View(dbContext.Persons.Include("Hobbies").ToList());
        }

        // GET: PersonsController/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                return View(dbContext.Persons.FirstOrDefault(p => p.Id == id));
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }

        }

        // GET: PersonsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PersonsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Person model)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                dbContext.Persons.Add(model);
                dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
                //}
                //else
                //{
                //    return View(model);
                //}
            }
            catch
            {
                return View(model);
            }
        }

        // GET: PersonsController/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                return View(dbContext.Persons.FirstOrDefault(p => p.Id == id));
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: PersonsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Person model)
        {
            try
            {
                dbContext.Persons.Update(model);
                dbContext.SaveChanges();
                return RedirectToAction("Index", "Persons");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }

        }

        // GET: PersonsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PersonsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
    }
}
