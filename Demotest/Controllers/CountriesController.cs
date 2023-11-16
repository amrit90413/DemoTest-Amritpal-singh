using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Demotest.Data;
using Demotest.Models;

namespace Demotest.Controllers
{
    public class CountriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CountriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Countries
        public async Task<IActionResult> Index()
        {
              return _context.Countries != null ? 
                          View(await _context.Countries.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Countries'  is null.");
        }

        // GET: Countries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }

            var countries = await _context.Countries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (countries == null)
            {
                return NotFound();
            }

            return View(countries);
        }

        // GET: Countries/Create
        public IActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,countryName")] Countries countries)
        {
            if (ModelState.IsValid)
            {
               
                var existingCountry = await _context.Countries
                    .FirstOrDefaultAsync(c => c.countryName == countries.countryName);

                if (existingCountry != null)
                {
                    // Country already exists, handle accordingly (e.g., display an error message)
                    ModelState.AddModelError("countryName", "Country already exists");
                    return View(countries);
                }

                // Country doesn't exist, proceed with adding it to the database
                _context.Add(countries);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(countries);
        }

        // GET: Countries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }

            var countries = await _context.Countries.FindAsync(id);
            if (countries == null)
            {
                return NotFound();
            }
            return View(countries);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
     
        public async Task<IActionResult> Edit(int id, [Bind("Id,countryName")] Countries countries)
        {
            if (id != countries.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Check if the updated country name already exists in the database
                var existingCountry = await _context.Countries
                    .FirstOrDefaultAsync(c => c.countryName == countries.countryName && c.Id != countries.Id);

                if (existingCountry != null)
                {
                    // Country with the updated name already exists, handle accordingly (e.g., display an error message)
                    ModelState.AddModelError("countryName", "Country name already exists");
                    return View(countries);
                }

                try
                {
                    _context.Update(countries);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountriesExists(countries.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(countries);
        }


        // GET: Countries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }

            var countries = await _context.Countries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (countries == null)
            {
                return NotFound();
            }

            return View(countries);
        }

        // POST: Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Countries == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Countries'  is null.");
            }
            var countries = await _context.Countries.FindAsync(id);
            if (countries != null)
            {
                _context.Countries.Remove(countries);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CountriesExists(int id)
        {
          return (_context.Countries?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
