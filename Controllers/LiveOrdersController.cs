using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ICream.Data;
using ICream.Models;

namespace ICream.Controllers
{
    public class LiveOrdersController : Controller
    {
        private readonly ICreamContext _context;

        public LiveOrdersController(ICreamContext context)
        {
            _context = context;
        }

        // GET: LiveOrders
        public async Task<IActionResult> Index()
        {
              return _context.LiveOrders != null ? 
                          View(await _context.LiveOrders.ToListAsync()) :
                          Problem("Entity set 'ICreamContext.LiveOrders'  is null.");
        }

        // GET: LiveOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LiveOrders == null)
            {
                return NotFound();
            }

            var liveOrders = await _context.LiveOrders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (liveOrders == null)
            {
                return NotFound();
            }

            return View(liveOrders);
        }

        // GET: LiveOrders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LiveOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Status,OrderDate,HebDate,Day,Holiday,Weather,Name,Email,Phone,City,Street,Apt,Total,IceCreams")] LiveOrders liveOrders)
        {
            if (ModelState.IsValid)
            {
                _context.Add(liveOrders);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(liveOrders);
        }

        // GET: LiveOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LiveOrders == null)
            {
                return NotFound();
            }

            var liveOrders = await _context.LiveOrders.FindAsync(id);
            if (liveOrders == null)
            {
                return NotFound();
            }
            return View(liveOrders);
        }

        // POST: LiveOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Status,OrderDate,HebDate,Day,Holiday,Weather,Name,Email,Phone,City,Street,Apt,Total,IceCreams")] LiveOrders liveOrders)
        {
            if (id != liveOrders.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(liveOrders);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LiveOrdersExists(liveOrders.Id))
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
            return View(liveOrders);
        }

        // GET: LiveOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LiveOrders == null)
            {
                return NotFound();
            }

            var liveOrders = await _context.LiveOrders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (liveOrders == null)
            {
                return NotFound();
            }

            return View(liveOrders);
        }

        // POST: LiveOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LiveOrders == null)
            {
                return Problem("Entity set 'ICreamContext.LiveOrders'  is null.");
            }
            var liveOrders = await _context.LiveOrders.FindAsync(id);
            if (liveOrders != null)
            {
                _context.LiveOrders.Remove(liveOrders);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LiveOrdersExists(int id)
        {
          return (_context.LiveOrders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
