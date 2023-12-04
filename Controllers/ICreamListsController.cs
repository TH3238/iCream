using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ICream.Data;
using ICream.Models;
using ICream.Services;
using ICream.Data;
using ICream.Models;
using ICream.Services;

public class iCreamListsController : Controller
{
    private readonly ICreamContext _context;
    private readonly ImaggaService _imaggaService;

    public iCreamListsController(ICreamContext context, ImaggaService imaggaService)
    {
        _context = context;
        _imaggaService = imaggaService ?? throw new ArgumentNullException(nameof(imaggaService));
    }

    // GET: iCreamLists
    public async Task<IActionResult> Index()
    {
        return _context.ICreamList != null ?
                      View(await _context.ICreamList.ToListAsync()) :
                      Problem("Entity set 'iCreamContext.iCreamList'  is null.");
    }

    // GET: iCreamLists/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.ICreamList == null)
        {
            return NotFound();
        }

        var iCreamList = await _context.ICreamList
            .FirstOrDefaultAsync(m => m.Id == id);
        if (iCreamList == null)
        {
            return NotFound();
        }

        return View(iCreamList);
    }

    // GET: iCreamLists/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: iCreamLists/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Description,Image,Price")] ICreamList iCreamList)
    {
        if (ModelState.IsValid)
        {
            // Check if the image is ice cream before adding it
            bool isIceCream = await _imaggaService.IsIceCream(iCreamList.Image);

            if (isIceCream)
            {
                _context.Add(iCreamList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Handle the case where the image is not ice cream
                ModelState.AddModelError("Image", "The provided image is not of an ice cream.");
            }
        }
        return View(iCreamList);
    }

    // GET: iCreamLists/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.ICreamList == null)
        {
            return NotFound();
        }

        var iCreamList = await _context.ICreamList.FindAsync(id);
        if (iCreamList == null)
        {
            return NotFound();
        }
        return View(iCreamList);
    }

    // POST: iCreamLists/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Image,Price")] ICreamList iCreamList)
    {
        if (id != iCreamList.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                bool isIceCream = await _imaggaService.IsIceCream(iCreamList.Image);

                if (isIceCream)
                {
                    _context.Update(iCreamList);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    // Handle the case where the image is not ice cream
                    ModelState.AddModelError("Image", "The provided image is not of an ice cream.");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!iCreamListExists(iCreamList.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
        return View(iCreamList);
    }

    // GET: iCreamLists/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.ICreamList == null)
        {
            return NotFound();
        }

        var iCreamList = await _context.ICreamList
            .FirstOrDefaultAsync(m => m.Id == id);
        if (iCreamList == null)
        {
            return NotFound();
        }

        return View(iCreamList);
    }

    // POST: iCreamLists/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.ICreamList == null)
        {
            return Problem("Entity set 'iCreamContext.iCreamList'  is null.");
        }
        var iCreamList = await _context.ICreamList.FindAsync(id);
        if (iCreamList != null)
        {
            _context.ICreamList.Remove(iCreamList);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool iCreamListExists(int id)
    {
        return (_context.ICreamList?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
