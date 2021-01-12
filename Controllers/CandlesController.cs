using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuntaNoastra_Buta_Camelia.Data;
using NuntaNoastra_Buta_Camelia.Models;

namespace NuntaNoastra_Buta_Camelia.Controllers
{
    public class CandlesController : Controller
    {
        private readonly ShopContext _context;

        public CandlesController(ShopContext context)
        {
            _context = context;
        }

        // GET: Candles
        public async Task<IActionResult> Index(string sortOrder, string searchString, string cautareDistrib)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            ViewData["NameFilter"] = searchString;
            ViewData["DistribuitorFilter"] = cautareDistrib;

            var candles = from b in _context.Candles
                          select b;
            if (!String.IsNullOrEmpty(searchString))
            {
                candles = candles.Where(l => l.Name.Contains(searchString));
            }
            if (!String.IsNullOrEmpty(cautareDistrib))
            {
                candles = candles.Where(l => l.Distribuitor.Contains(cautareDistrib));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    candles = candles.OrderByDescending(b => b.Name);
                    break;
                case "Price":
                    candles = candles.OrderBy(b => b.Price);
                    break;
                case "price_desc":
                    candles = candles.OrderByDescending(b => b.Price);
                    break;
                default:
                    candles = candles.OrderBy(b => b.Name);
                    break;
            }
            return View(await candles.AsNoTracking().ToListAsync());
        }


        // GET: Candles/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var candle = await _context.Candles
             .Include(s => s.Orders)
             .ThenInclude(e => e.Customer)
             .AsNoTracking()
             .FirstOrDefaultAsync(m => m.ID == id);
            if (candle == null)
            {
                return NotFound();
            }

            return View(candle);
        }

        // GET: Candles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Candles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Distribuitor,Price")] Candle candle)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(candle);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex*/)
            {

                ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists ");
            }
            return View(candle);
        }

        // GET: Candles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candle = await _context.Candles.FindAsync(id);
            if (candle == null)
            {
                return NotFound();
            }
            return View(candle);
        }

        // POST: Candles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var clientToUpdate = await _context.Candles.FirstOrDefaultAsync(s => s.ID == id);
            if (await TryUpdateModelAsync<Candle>(
            clientToUpdate,
            "",
            s => s.Distribuitor, s => s.Name, s => s.Price))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists");
                }
            }
            return View(clientToUpdate);
        }

        // GET: Candles/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candle = await _context.Candles
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (candle == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                "Delete failed. Try again";
            }

            return View(candle);
        }


        // POST: Candles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var candle = await _context.Candles.FindAsync(id);
            if (candle == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Candles.Remove(candle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {

                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool CandleExists(int id)
        {
            return _context.Candles.Any(e => e.ID == id);
        }
    }
}
