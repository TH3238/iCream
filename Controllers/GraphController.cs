using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ICream.Data;
using Newtonsoft.Json;

[Route("Graph")]
public class GraphController : Controller
{
    private readonly ICreamContext _context;

    public GraphController(ICreamContext context)
    {
        _context = context;
    }

    [HttpGet("")]
    public IActionResult Index(string selectedIceCream)
    {
        var orderDetails = _context.OrderDetails.ToList();

        // Graph for Total
        var totalGraphData = orderDetails
            .GroupBy(o => o.Date)
            .Select(g => new { Date = g.Key, Total = g.Sum(o => o.Total), IceCreamCount = 0 })
            .OrderBy(o => o.Date)
            .ToList();

        // Graph for selected ice cream
        var selectedIceCreamGraphData = orderDetails
            .SelectMany(o => o.IceCreams.Split(","), (o, iceCream) => new { Date = o.Date, IceCream = iceCream.Trim() })
            .Select(o => new
            {
                o.Date,
                o.IceCream,
                Quantity = ParseIceCreamQuantity(o.IceCream)
            })
            .Where(o => o.Quantity > 0)
            .GroupBy(o => o.Date)
            .Select(g => new { Date = g.Key, Total = 0f, IceCreamCount = g.Sum(o => o.Quantity) })
            .OrderBy(o => o.Date)
            .ToList();

        // Combine the two datasets into a single list
        var combinedGraphData = totalGraphData
            .Concat(selectedIceCreamGraphData)
            .GroupBy(o => o.Date)
            .Select(g => new { Date = g.Key, Total = g.Sum(o => o.Total), IceCreamCount = g.Sum(o => o.IceCreamCount) })
            .OrderBy(o => o.Date);

        // Log the selectedIceCreamGraphData to ensure it has data
        Console.WriteLine($"Selected Ice Cream Graph Data: {JsonConvert.SerializeObject(selectedIceCreamGraphData)}");

        // Get the list of ice cream flavors from ICreamList table
        var iceCreamList = _context.ICreamList.Select(i => i.Description).Distinct().ToList();

        ViewBag.IceCreamList = iceCreamList;
        ViewBag.SelectedIceCream = selectedIceCream;

        return View(combinedGraphData);
    }

    private int ParseIceCreamQuantity(string iceCream)
    {
        // Example input: "Chocolate (2)"
        var openParenthesisIndex = iceCream.LastIndexOf('(');
        var closeParenthesisIndex = iceCream.LastIndexOf(')');

        if (openParenthesisIndex != -1 && closeParenthesisIndex != -1 && closeParenthesisIndex > openParenthesisIndex)
        {
            var quantityStr = iceCream.Substring(openParenthesisIndex + 1, closeParenthesisIndex - openParenthesisIndex - 1).Trim();
            if (int.TryParse(quantityStr, out var quantity))
            {
                return quantity;
            }
        }

        return 0;
    }

}
