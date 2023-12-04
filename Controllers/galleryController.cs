using ICream.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ICream.Controllers
{
    public class galleryController : Controller
    {
        private readonly ICreamContext _context;

        public galleryController(ICreamContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            var iceCreams = _context.ICreamList.ToList(); 
            return View(iceCreams);
        }

        // GET: galleryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: galleryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: galleryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: galleryController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: galleryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: galleryController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: galleryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
