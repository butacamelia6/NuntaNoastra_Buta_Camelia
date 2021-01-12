using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuntaNoastra_Buta_Camelia.Data;
using NuntaNoastra_Buta_Camelia.Models;
using NuntaNoastra_Buta_Camelia.Models.ViewItem;
using NuntaNoastra_Buta_Camelia.ViewItem;

namespace NuntaNoastra_Buta_Camelia.Controllers
{
    public class DistribuitorsController : Controller
    {
        private readonly ShopContext _context;

        public DistribuitorsController(ShopContext context)
        {
            _context = context;
        }

        // GET: Distribuitors
        public async Task<IActionResult> Index(int? id, int? candleID)
        {
            var viewModel = new DistribuitorIndexData();
            viewModel.Distribuitors = await _context.Distribuitors
            .Include(i => i.DistribuitorCandles)
            .ThenInclude(i => i.Candle)
            .ThenInclude(i => i.Orders)
            .ThenInclude(i => i.Customer)
            .AsNoTracking()
            .OrderBy(i => i.DistribuitorName)
            .ToListAsync();
            if (id != null)
            {
                ViewData["DistribuitorID"] = id.Value;
                Distribuitor distribuitor = viewModel.Distribuitors.Where(
                i => i.ID == id.Value).Single();
                viewModel.Candles = distribuitor.DistribuitorCandles.Select(s => s.Candle);
            }
            if (candleID != null)
            {
                ViewData["CandleID"] = candleID.Value;
                viewModel.Orders = viewModel.Candles.Where(
                x => x.ID == candleID).Single().Orders;
            }
            return View(viewModel);
        }
        // GET: Distribuitors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distribuitor = await _context.Distribuitors
                .FirstOrDefaultAsync(m => m.ID == id);
            if (distribuitor == null)
            {
                return NotFound();
            }

            return View(distribuitor);
        }

        // GET: Distribuitors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Distribuitors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,DistribuitorName,Adress")] Distribuitor distribuitor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(distribuitor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(distribuitor);
        }

        // GET: Distribuitors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var distribuitor = await _context.Distribuitors
            .Include(i => i.DistribuitorCandles).ThenInclude(i => i.Candle)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == id);
            if (distribuitor == null)
            {
                return NotFound();
            }
            PopulateDistribuitorCandleData(distribuitor);
            return View(distribuitor);

        }
        private void PopulateDistribuitorCandleData(Distribuitor distribuitor)
        {
            var allCandles = _context.Candles;
            var distribuitorCandles = new HashSet<int>(distribuitor.DistribuitorCandles.Select(c => c.CandleID));
            var viewModel = new List<DistribuitorCandleData>();
            foreach (var candle in allCandles)
            {
                viewModel.Add(new DistribuitorCandleData
                {
                    CandleID = candle.ID,
                    Name = candle.Name,
                    IsPublished = distribuitorCandles.Contains(candle.ID)
                });
            }
            ViewData["Candles"] = viewModel;
        }

        // POST: Distribuitors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedCandles)
        {
            if (id == null)
            {
                return NotFound();
            }
            var candledataToUpdate = await _context.Distribuitors
            .Include(i => i.DistribuitorCandles)
            .ThenInclude(i => i.Candle)
            .FirstOrDefaultAsync(m => m.ID == id);
            if (await TryUpdateModelAsync<Distribuitor>(candledataToUpdate, "", i => i.DistribuitorName, i => i.Adress))
            {
                UpdateDistribuitorCandles(selectedCandles, candledataToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {

                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists, ");
                }
                return RedirectToAction(nameof(Index));
            }
            UpdateDistribuitorCandles(selectedCandles, candledataToUpdate);
            PopulateDistribuitorCandleData(candledataToUpdate);
            return View(candledataToUpdate);
        }
        private void UpdateDistribuitorCandles(string[] selectedCandles, Distribuitor candledataToUpdate)
        {
            if (selectedCandles == null)
            {
                candledataToUpdate.DistribuitorCandles = new List<DistribuitorCandle>();
                return;
            }
            var selectedCandlesHS = new HashSet<string>(selectedCandles);
            var distribuitorCandles = new HashSet<int>
            (candledataToUpdate.DistribuitorCandles.Select(c => c.Candle.ID));
            foreach (var candle in _context.Candles)
            {
                if (selectedCandlesHS.Contains(candle.ID.ToString()))
                {
                    if (!distribuitorCandles.Contains(candle.ID))
                    {
                        candledataToUpdate.DistribuitorCandles.Add(new DistribuitorCandle
                        {
                            DistribuitorID = candledataToUpdate.ID,
                            CandleID = candle.ID
                        });
                    }
                }
                else
                {
                    if (distribuitorCandles.Contains(candle.ID))
                    {
                        DistribuitorCandle candleToRemove = candledataToUpdate.DistribuitorCandles.FirstOrDefault(i => i.CandleID == candle.ID);
                        _context.Remove(candleToRemove);
                    }
                }
            }
        }
        // GET: Distribuitors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distribuitor = await _context.Distribuitors
                .FirstOrDefaultAsync(m => m.ID == id);
            if (distribuitor == null)
            {
                return NotFound();
            }

            return View(distribuitor);
        }

        // POST: Distribuitors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var distribuitor = await _context.Distribuitors.FindAsync(id);
            _context.Distribuitors.Remove(distribuitor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DistribuitorExists(int id)
        {
            return _context.Distribuitors.Any(e => e.ID == id);
        }
    }
}
