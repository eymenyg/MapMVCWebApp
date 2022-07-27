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
using System.Text.Json;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace MapMVCWebApp.Controllers
{
    public class LocationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LocationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Locations
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        // POST: Locations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Latitude,Longitude")] Location location)
        {
            if (ModelState.IsValid)
            {
                _context.Add(location);
                await _context.SaveChangesAsync();
                return Json(JsonSerializer.Serialize(location));
            }
            return Json("error");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFromList([DataSourceRequest] DataSourceRequest request, Location location)
        {

            if (ModelState.IsValid)
            {
                using (var context = _context)
                {
                    var newLocation = new Location
                    {
                        Title = location.Title,
                        Latitude = location.Latitude,
                        Longitude = location.Longitude
                    };
                    _context.Add(newLocation);
                    await _context.SaveChangesAsync();
                    location.Id = newLocation.Id;
                }
                return Json(new[] { location }.ToDataSourceResult(request, ModelState));
            }
            return Json("error");
            
        }

        public async Task<IActionResult> ReadFromList([DataSourceRequest] DataSourceRequest request)
        {
            using (var context = _context)
            {
                IQueryable<Location> locations = context.Location;
                DataSourceResult result = await locations.ToDataSourceResultAsync(request);
                return Json(result);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateFromList([DataSourceRequest] DataSourceRequest request, Location location)
        {

            if (ModelState.IsValid)
            {
                using (var context = _context)
                {
                    var newLocation = new Location
                    {
                        Id = location.Id,
                        Title = location.Title,
                        Latitude = location.Latitude,
                        Longitude = location.Longitude
                    };

                    context.Location.Attach(newLocation);
                    context.Entry(newLocation).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
            }
            return Json(new[] { location }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DestroyFromList([DataSourceRequest] DataSourceRequest request, Location location)
        {
            if (ModelState.IsValid)
            {
                using (var context = _context)
                {

                    var newLocation = new Location
                    {
                        Id = location.Id,
                        Title = location.Title,
                        Latitude = location.Latitude,
                        Longitude = location.Longitude
                    };
                    context.Location.Attach(newLocation);
                    context.Location.Remove(newLocation);
                    await _context.SaveChangesAsync();
                }
            }
            return Json(new[] { location }.ToDataSourceResult(request, ModelState));
        }

        private bool LocationExists(int id)
        {
          return (_context.Location?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
