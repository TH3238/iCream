using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ICream.Data;
using ICream.Models;
using Newtonsoft.Json.Linq;
using ICream.Services;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http;


namespace ICream.Controllers
{
    public class OrderDetailsController : Controller
    {
        private readonly ICreamContext _context;
        private readonly AddressValidationGateway _addressValidationGateway;



        public OrderDetailsController(ICreamContext context)
        {
            _context = context;
            _addressValidationGateway = new AddressValidationGateway();


        }


        // GET: OrderDetails
        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate, string searchTerm)
        {
            IQueryable<OrderDetails> orders = _context.OrderDetails;

            // Date range filtering
            if (startDate != null)
            {
                orders = orders.Where(o => o.Date >= startDate);
            }

            if (endDate != null)
            {
                // Assuming the Date property is inclusive, if not, you may need to adjust the comparison.
                orders = orders.Where(o => o.Date <= endDate.Value.AddDays(1));
            }

            // Search filtering (by name, email, or phone)
            if (!string.IsNullOrEmpty(searchTerm))
            {
                orders = orders.Where(o =>
                    o.Name.Contains(searchTerm) ||
                    o.Email.Contains(searchTerm) ||
                    o.Phone.Contains(searchTerm)
                );
            }

            // Set ViewBag values for retaining the filter values in the form
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;
            ViewBag.SearchTerm = searchTerm;

            return View(await orders.ToListAsync());
        }


        // GET: OrderDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.OrderDetails == null)
            {
                return NotFound();
            }

            var orderDetails = await _context.OrderDetails
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderDetails == null)
            {
                return NotFound();
            }

            return View(orderDetails);
        }

        // GET: OrderDetails/Create
        public IActionResult Create(float total,string iceCreams)
        {
            var orderDetails = new OrderDetails(); // Assuming OrderDetails is your model
            TempData["Date"] = DateTime.Now;
            List<string> selectedIceCreams;

            if (!string.IsNullOrEmpty(iceCreams))
            {
                try
                {
                    selectedIceCreams = JsonConvert.DeserializeObject<List<string>>(iceCreams);
                }
                catch (JsonReaderException)
                {
                    selectedIceCreams = new List<string>();
                }
            }
            else
            {
                selectedIceCreams = new List<string>();

            }

            string iceCreamString = string.Join(",", selectedIceCreams);

            SetReadOnlyValues(orderDetails, total, iceCreamString);
            return View();
        }

        // POST: OrderDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,HebDate,Day,Holiday,Weather,Name,Email,Phone,City,Street,Apt,IceCreams,Total")] OrderDetails orderDetails)
        {
            if (ModelState.IsValid)
            {
                bool isValidAddress = await _addressValidationGateway.CheckAddress(orderDetails.City, orderDetails.Street);

                if (!isValidAddress)
                {
                    ModelState.AddModelError("City", "The provided address is not valid.");
                    ModelState.AddModelError("Street", "The provided street is not valid.");
                    TempData["Date"] = DateTime.Now;

                    SetReadOnlyValues(orderDetails, orderDetails.Total,orderDetails.IceCreams);
                    return View(orderDetails);
                }
                orderDetails.Date = DateTime.Now;

                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5036/"); // Replace with your Web API URL

                    HttpResponseMessage response = await client.GetAsync($"api/weather?city={orderDetails.City}"); // Replace with your API endpoint and parameters

                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        WeatherData weather = JsonConvert.DeserializeObject<WeatherData>(responseData);
                        string weatherString = $"{weather.Temperature}°C,\n{weather.Description},\nHumidity: {weather.Humidity}%,\nWind: {weather.WindSpeed} km/h";
                        orderDetails.Weather = weatherString;
                    }
                    else
                    {
                        return View("Error"); // Handle errors when the API call is not successful
                    }
                }
                _context.Add(orderDetails);
                await _context.SaveChangesAsync();
                
                // Save to LiveOrders table
                var liveOrder = new LiveOrders(orderDetails);
                _context.Add(liveOrder);
                await _context.SaveChangesAsync();

                return RedirectToAction("ThankYou");
            }

            TempData["Date"] = DateTime.Now;
            SetReadOnlyValues(orderDetails, orderDetails.Total, orderDetails.IceCreams);
            return View(orderDetails);
        }



        // GET: OrderDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.OrderDetails == null)
            {
                return NotFound();
            }

            var orderDetails = await _context.OrderDetails.FindAsync(id);
            if (orderDetails == null)
            {
                return NotFound();
            }
            return View(orderDetails);
        }

        // POST: OrderDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,HebDate,Day,Holiday,Weather,Name,Email,Phone,City,Street,Apt,IceCreams,Total")] OrderDetails orderDetails)
        {
            if (id != orderDetails.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderDetailsExists(orderDetails.Id))
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
            return View(orderDetails);
        }

        // GET: OrderDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.OrderDetails == null)
            {
                return NotFound();
            }

            var orderDetails = await _context.OrderDetails
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderDetails == null)
            {
                return NotFound();
            }

            return View(orderDetails);
        }

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.OrderDetails == null)
            {
                return Problem("Entity set 'ICreamContext.OrderDetails'  is null.");
            }
            var orderDetails = await _context.OrderDetails.FindAsync(id);
            if (orderDetails != null)
            {
                _context.OrderDetails.Remove(orderDetails);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderDetailsExists(int id)
        {
          return (_context.OrderDetails?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private void SetReadOnlyValues(OrderDetails orderDetails, float total, string iceCreams)
        {
            var HebCalModel = new HebCalAdapter();
            var dateData = HebCalModel.Check();
            JObject json = JObject.Parse(dateData);
            int holidayValue = (int)json["holiday"];
            string HebDate = (string)json["hebrewDate"];

            Holiday holiday = (Holiday)holidayValue;
            string holidayName = Enum.GetName(typeof(Holiday), holiday);

            ViewBag.Total = total;

            // Create a dictionary to store ice cream counts
            Dictionary<string, int> iceCreamCounts = new Dictionary<string, int>();

            // Split the ice creams string into an array
            string[] iceCreamArray = iceCreams.Split(',');

            // Count occurrences of each ice cream
            foreach (var iceCream in iceCreamArray)
            {
                if (iceCreamCounts.ContainsKey(iceCream))
                {
                    iceCreamCounts[iceCream]++;
                }
                else
                {
                    iceCreamCounts[iceCream] = 1;
                }
            }

            // Create a formatted string with ice cream names and quantities
            string formattedIceCreams = string.Join(", ", iceCreamCounts.Select(kv => $"{kv.Key}({kv.Value})"));

            ViewBag.IceCreams = formattedIceCreams;

            TempData["HebDate"] = HebDate;
            TempData["Holiday"] = holidayName;
            TempData["Day"] = DateTime.Today.DayOfWeek.ToString();
        }

        public IActionResult ThankYou()
        {
            return View();
        }

    }

}
