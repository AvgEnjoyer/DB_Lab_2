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
    public class BrandsController : Controller
    {
        private const string ERR_DEV_EXISTS = "Введений розробник вже доданий";
        private readonly DBLab2Context _context;

        public BrandsController(DBLab2Context context)
        {
            _context = context;
        }

        // GET: Brands
        public async Task<IActionResult> Index()
        {
            var dBLab2Context = _context.Brands.Include(b => b.Country);
            return View(await dBLab2Context.ToListAsync());
        }

        //// GET: Brands/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null || _context.Brands == null)
        //    {
        //        return NotFound();
        //    }

        //    var brand = await _context.Brands
        //        .Include(b => b.Country)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (brand == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(brand);
        //}

        // GET: Brands/Create
        public IActionResult Create()
        {
            ViewBag.CountryList = new SelectList(_context.Countries.ToList(), "Id", "Name");
            return View();
        }

        // POST: Brands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Brand brand)
        {

            bool duplicate = await _context.Brands.AnyAsync(d => d.Name.Equals(brand.Name));

            if (duplicate)
            {
                ModelState.AddModelError("Name", ERR_DEV_EXISTS);
            }



            if (ModelState.IsValid)
            {
                _context.Add(brand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CountryList = new SelectList(_context.Countries, "Id", "Name", brand.CountryId);
            return View(brand);
        }

        // GET: Brands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Brands == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            ViewBag.CountryList = new SelectList(_context.Countries, "Id", "Name", brand.CountryId);
            return View(brand);
        }

        // POST: Brands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Brand brand)
        {
            if (id != brand.Id)
            {
                return NotFound();
            }



            bool duplicate = await _context.Brands.AnyAsync(d => d.Name.Equals(brand.Name) && d.CountryId == brand.CountryId && d.Id != brand.Id);

            if (duplicate)
            {
                ModelState.AddModelError("Name", ERR_DEV_EXISTS);
            }







            if (ModelState.IsValid)
            {
                //try
                //{
             _context.Update(brand);
             await _context.SaveChangesAsync();
                //}
                //catch (DbUpdateConcurrencyException)
                //{
                //    if (!BrandExists(brand.Id))
                //    {
                //        return NotFound();
                //    }
                //    else
                //    {
                //        throw;
                //    }
                //}
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CountryList = new SelectList(_context.Countries, "Id", "Name", brand.CountryId);
            return View(brand);
        }

        //// GET: Brands/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.Brands == null)
        //    {
        //        return NotFound();
        //    }

        //    var brand = await _context.Brands
        //        .Include(b => b.Country)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (brand == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(brand);
        //}

        // POST: Brands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Brands == null)
            {
                return Problem("Entity set 'DBLab2Context.Brands'  is null.");
            }
            var brand = await _context.Brands.FindAsync(id);
            if (brand != null)
            {
                _context.Brands.Remove(brand);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //private bool BrandExists(int id)
        //{
        //  return (_context.Brands?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
