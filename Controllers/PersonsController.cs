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
    public class PersonsController : Controller
    {
        private readonly DBLab2Context _context;

        public PersonsController(DBLab2Context context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Persons.ToListAsync());
        }
    }
}
