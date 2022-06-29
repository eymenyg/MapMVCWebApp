using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MapMVCWebApp.Data;
using MapMVCWebApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace MapMVCWebApp.Controllers
{
    public class LocationModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LocationModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LocationModels
        [Authorize]
        public async Task<IActionResult> Index(string searchQuery)
        {
            if(!String.IsNullOrEmpty(searchQuery))
            {
                return _context.LocationModel != null ?
                        View(await _context.LocationModel.
                        Where(j => j.Title.Contains(searchQuery)).ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.LocationModel'  is null.");
            }
              return _context.LocationModel != null ? 
                          View(await _context.LocationModel.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.LocationModel'  is null.");
        }

        // GET: LocationModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LocationModel == null)
            {
                return NotFound();
            }

            var locationModel = await _context.LocationModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (locationModel == null)
            {
                return NotFound();
            }

            return View(locationModel);
        }

        // GET: LocationModels/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: LocationModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Latitude,Longitude")] LocationModel locationModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(locationModel);
                await _context.SaveChangesAsync();
                return Json("success");
            }
            return Json("error");
        }

        // GET: LocationModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LocationModel == null)
            {
                return NotFound();
            }

            var locationModel = await _context.LocationModel.FindAsync(id);
            if (locationModel == null)
            {
                return NotFound();
            }
            return View(locationModel);
        }

        // POST: LocationModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Latitude,Longitude")] LocationModel locationModel)
        {
            if (id != locationModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(locationModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationModelExists(locationModel.Id))
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
            return View(locationModel);
        }

        // GET: LocationModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LocationModel == null)
            {
                return NotFound();
            }

            var locationModel = await _context.LocationModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (locationModel == null)
            {
                return NotFound();
            }

            return View(locationModel);
        }

        // POST: LocationModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LocationModel == null)
            {
                return Problem("Entity set 'ApplicationDbContext.LocationModel'  is null.");
            }
            var locationModel = await _context.LocationModel.FindAsync(id);
            if (locationModel != null)
            {
                _context.LocationModel.Remove(locationModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocationModelExists(int id)
        {
          return (_context.LocationModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
