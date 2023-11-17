using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Demotest.Data;
using Demotest.Models;
using System.Diagnostics.Metrics;


namespace Demotest.Controllers
{
    public class StatesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: States
        public IActionResult Index()
        {
            try
            {
                var data = (from u in _context.States
                            join c in _context.Countries on u.CountryId equals c.Id
                            select new countrystateviewmodel()
                            {
                                Id = u.Id,
                                countryName = c.countryName,
                                StateName = u.StateName
                            }).ToList();
                return View(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        // GET: States/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                var states = await (from u in _context.States
                                    join c in _context.Countries on u.CountryId equals c.Id
                                    select new countrystateviewmodel()
                                    {
                                        Id = u.Id,
                                        countryName = c.countryName,
                                        StateName = u.StateName
                                    }).FirstOrDefaultAsync(m => m.Id == id);

                if (states == null)
                {
                    return NotFound();
                }

                return View(states);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal Server Error");
            }

        }

        // GET: States/Create
        public IActionResult Create()

        {
            var country = _context.Countries.ToList();
            
            ViewBag.Country = new SelectList(country, "Id", "countryName");
            return View();
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SId,StateName,CountryId")] States state)
        {
            ModelState.Remove("SId");

            if (ModelState.IsValid)
            {
                // Check if the state with the same name and country exists
                var existingState = await _context.States
                    .Where(s => s.StateName == state.StateName && s.CountryId == state.CountryId)
                    .FirstOrDefaultAsync();

                if (existingState != null)
                {
                    // State already exists, handle accordingly (e.g., show a message)
                    ModelState.AddModelError(string.Empty, "State already exists for the selected country.");
                    return View(state);
                }

                // State doesn't exist, proceed with adding
                _context.Add(state);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(state);
        }

        // GET: States/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.States == null)
            {
                return NotFound();
            }

            var states = await _context.States.FindAsync(id);

            if (states == null)
            {
                return NotFound();
            }

            var country = _context.Countries.ToList();
            ViewBag.Country = new SelectList(country, "Id", "countryName");

            return View(states);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StateName,CountryId")] States states)
        {
            if (id != states.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Check if the updated state name already exists in the database
                var existingState = await _context.States
                    .FirstOrDefaultAsync(s => s.StateName == states.StateName && s.Id != states.Id);

                if (existingState != null)
                {
                    // State with the updated name already exists, handle accordingly (e.g., display an error message)
                    ModelState.AddModelError("StateName", "State name already exists");
                    return View(states);
                }

                try
                {
                    _context.Update(states);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StatesExists(states.Id))
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

            return View(states);
        }
        // GET: Countries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var states = await (from u in _context.States
                                join c in _context.Countries on u.CountryId equals c.Id
                                select new countrystateviewmodel()
                                {
                                    Id = u.Id,
                                    countryName = c.countryName,
                                    StateName = u.StateName
                                }).FirstOrDefaultAsync(m => m.Id == id);

            if (states == null)
            {
                return NotFound();
            }

          

            return View(states);
        }

        
        // POST: States/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.States == null)
            {
                return Problem("Entity set 'ApplicationDbContext.States'  is null.");
            }
            var states = await _context.States.FindAsync(id);
            if (states != null)
            {
                _context.States.Remove(states);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StatesExists(int id)
        {
          return (_context.States?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
