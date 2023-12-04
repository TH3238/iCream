using ICream.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ICream.Controllers
{
    public class indexController : Controller
    {
        private readonly ICreamContext _context;

        public indexController(ICreamContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            var iceCreams = _context.ICreamList.ToList(); 
            return View(iceCreams);
        }

        // GET: indexController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: indexController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: indexController/Create
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

        // GET: indexController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: indexController/Edit/5
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

        // GET: indexController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: indexController/Delete/5
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
