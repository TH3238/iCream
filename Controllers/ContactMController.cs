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
    public class ContactMController : Controller
    {
        private readonly ICreamContext _context;

        public ContactMController(ICreamContext context)
        {
            _context = context;
        }

        // GET: ContactM
        public async Task<IActionResult> Index()
        {
              return _context.ContactM != null ? 
                          View(await _context.ContactM.ToListAsync()) :
                          Problem("Entity set 'ICreamContext.ContactM'  is null.");
        }

        // GET: ContactM/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ContactM == null)
            {
                return NotFound();
            }

            var contactM = await _context.ContactM
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactM == null)
            {
                return NotFound();
            }

            return View(contactM);
        }

        // GET: ContactM/Create
        public IActionResult Create()
        {
            var ContactM= new ContactM(); 
            TempData["Datec"] = DateTime.Now;
            return View();
        }

        // POST: ContactM/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,Name,Email,Subject,Message")] ContactM contactM)
        {
            if (ModelState.IsValid)
            {
                contactM.Date = DateTime.Now;

                _context.Add(contactM);
                await _context.SaveChangesAsync();
                return RedirectToAction("ThankYou");
            }
            TempData["Datec"] = DateTime.Now;

            return View(contactM);
        }

        // GET: ContactM/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ContactM == null)
            {
                return NotFound();
            }

            var contactM = await _context.ContactM.FindAsync(id);
            if (contactM == null)
            {
                return NotFound();
            }
            return View(contactM);
        }

        // POST: ContactM/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Name,Email,Subject,Message")] ContactM contactM)
        {
            if (id != contactM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contactM);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactMExists(contactM.Id))
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
            return View(contactM);
        }

        // GET: ContactM/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ContactM == null)
            {
                return NotFound();
            }

            var contactM = await _context.ContactM
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactM == null)
            {
                return NotFound();
            }

            return View(contactM);
        }

        // POST: ContactM/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ContactM == null)
            {
                return Problem("Entity set 'ICreamContext.ContactM'  is null.");
            }
            var contactM = await _context.ContactM.FindAsync(id);
            if (contactM != null)
            {
                _context.ContactM.Remove(contactM);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactMExists(int id)
        {
          return (_context.ContactM?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public IActionResult ThankYou()
        {
            return View();
        }
    }
}
