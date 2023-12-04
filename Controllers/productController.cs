using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ICream.Data;

namespace ICream.Controllers
{
    public class productController : Controller
    {
        private readonly ICreamContext _context; 

        public productController(ICreamContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            var iceCreams = _context.ICreamList.ToList(); // Replace 'icreamlist' with your actual model name
            return View(iceCreams);
        }

        // GET: productController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: productController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: productController/Create
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

        // GET: productController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: productController/Edit/5
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

        // GET: productController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: productController/Delete/5
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
