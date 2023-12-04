using Microsoft.AspNetCore.Mvc;

public class ManagerController : Controller
{
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string password)
    {
        // Check if the entered password is correct
        if (password == "2024icream")
        {
            // Password is correct, redirect to the Dashboard controller's Index action
            return RedirectToAction("Index", "Dashboard");
        }
        else
        {
            // Password is incorrect, display an error message
            ViewBag.ErrorMessage = "Invalid password. Please try again.";
            return View();
        }
    }
}

