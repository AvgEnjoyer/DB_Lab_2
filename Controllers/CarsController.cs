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
    public class CarsController : Controller
    {
        private const string ERR_SOFT_EXISTS = "Такий автомобіль вже доданий";
        private readonly DBLab2Context _context;

        public CarsController(DBLab2Context context)
        {
            _context = context;
        }

        // GET: Cars
        public async Task<IActionResult> Index(int id,bool purchased)
        {
            ViewBag.BrandId = id;
            List<Car> cars = new List<Car>();   
            if(purchased)
            {
                ViewBag.Purchased = 1;
            }

            if (id == 0)
            {
                cars = await _context.Cars.Include(s => s.Brand).ToListAsync();
            }
            else
            {
                ViewBag.Brand =_context.Brands.Find(id).Name;
                cars =await _context.Cars.Where(s=>s.BrandId==id).Include(c => c.Brand).ToListAsync();
            }
            return View(cars);
        }   

        //// GET: Cars/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null || _context.Cars == null)
        //    {
        //        return NotFound();
        //    }

        //    var car = await _context.Cars
        //        .Include(c => c.Brand)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (car == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(car);
        //}

        // GET: Cars/Create
        public IActionResult Create(int brandId)
        {
            ViewBag.BrandId = brandId;
            if(brandId == 0)
            ViewBag.BrandList = new SelectList(_context.Brands, "Id", "Name");
            ViewBag.BrandList = brandId == 0 ?
                new SelectList(_context.Brands, "Id", "Name") :
                new SelectList(new List<Brand>() { _context.Brands.Find(brandId) }, "Id", "Name");
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Car car)
        {
            ViewBag.BrandId=car.BrandId;
            ViewBag.Brand = _context.Brands.Find(car.BrandId);
            bool duplicate = _context.Cars.Any(s => s.BrandId == car.BrandId && s.Name.Equals(car.Name));
            if (duplicate)
            {
                ModelState.AddModelError("Name", ERR_SOFT_EXISTS);
            }
            if (ModelState.IsValid)
            {
                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = car.BrandId });
            }
            ViewBag.BrandList = new SelectList(_context.Brands, "Id", "Name", car.BrandId);
            return View(car);
        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id, int brandId)
        {
            if (id == null || _context.Cars == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            //ViewBag.BrandId = brandId;
            //ViewBag.BrandList = new SelectList(_context.Brands, "Id", "Name", car.BrandId);



            ViewBag.BrandList = new SelectList(_context.Brands, "Id", "Name");
            ViewBag.BrandList = brandId == 0 ?
                new SelectList(_context.Brands, "Id", "Name") :
                new SelectList(new List<Brand>() { _context.Brands.Find(brandId) }, "Id", "Name");





            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Car car, int brandId)
        {
            ViewBag.BrandId = brandId;
            bool duplicate = _context.Cars.Any(s => s.Id != car.Id && s.BrandId == car.BrandId && s.Name.Equals(car.Name));

            if (duplicate)
            {
                ModelState.AddModelError("Name", ERR_SOFT_EXISTS);
            }

            if (ModelState.IsValid)
            {
                
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index),new {id=brandId });
            }
            ViewBag.BrandList = new SelectList(_context.Brands, "Id", "Name", car.BrandId);
            return View(car);
        }

        //// GET: Cars/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.Cars == null)
        //    {
        //        return NotFound();
        //    }

        //    var car = await _context.Cars
        //        .Include(c => c.Brand)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (car == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(car);
        //}

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cars == null)
            {
                return Problem("Entity set 'DBLab2Context.Cars'  is null.");
            }
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
          return (_context.Cars?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
