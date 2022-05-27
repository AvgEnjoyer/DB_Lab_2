using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DB_Lab_2;

namespace DB_Lab_2.Controllers
{
    public class PurchasesController : Controller
    {
        private readonly DBLab2Context _context;

        public PurchasesController(DBLab2Context context)
        {
            _context = context;
        }

        // GET: Purchases
        public async Task<IActionResult> Index(int id)
        {
            List<Purchase> purchases;
            List<Car> cars;

            ViewBag.PersonId = id;

            if (id == 0)
            {
                purchases = await _context.Purchases.Include(p => p.Person).Include(p => p.Car).ToListAsync();
            }
            else
            {
                ViewBag.Person = _context.Persons.Find(id).Email;
                purchases = await _context.Purchases.Where(p => p.PersonId == id).Include(p => p.Car).ToListAsync();
            }

            foreach (var p in purchases)
            {
                cars= await _context.Cars.Where(s => s.Id == p.CarId).Include(s => s.Brand).ToListAsync();
                p.Car = cars[0];
            }

            return View(purchases);
        }

        public IActionResult Purchase(int carId, int brandId)
        {
            FillViewBag(carId, brandId);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Purchase(Person model, int carId, int brandId)
        {
            var person = await _context.Persons.FirstOrDefaultAsync(c => c.Email.Equals(model.Email));
            bool duplicate = person == null ? false : _context.Purchases.Any(p => p.CarId == carId && p.PersonId == person.Id);

            if (duplicate)
            {
                ModelState.AddModelError("Email", "Ви вже придбали цей продукт");
            }

            if (ModelState.IsValid)
            {
                if (person == null)
                {
                    person = model;
                    await _context.Persons.AddAsync(person);
                    await _context.SaveChangesAsync();
                }

                var purchase = new Purchase() { PersonId = person.Id, CarId = carId, Date = DateTime.Now };
                await _context.Purchases.AddAsync(purchase);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Cars", new { id = brandId, purchased = true });
            }

            FillViewBag(carId, brandId);
            return View(model);
        }





        public void FillViewBag(int carId, int brandId)
        {
            ViewBag.BrandId = brandId;
            ViewBag.CarId = carId;
            ViewBag.Car= _context.Cars.Find(carId).Name;
        }
    }
}
