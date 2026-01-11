using Microsoft.AspNetCore.Mvc;

public class HomeController: Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult AboutUs()
    {
        return View();
    }

    public IActionResult Products()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }
}