using Microsoft.AspNetCore.Mvc;

public class HomeController: Controller
{
    IProductService _productService;
    IImageService _imageService;
    ISliderService _sliderService;
    IFormService _formService;
    public HomeController(IProductService productService, IImageService imageService, ISliderService sliderService, IFormService formService)
    {
        _productService = productService;
        _imageService = imageService;
        _sliderService = sliderService;
        _formService = formService;
    }
    public IActionResult Index()
    {
        List<SliderModel> sliders = _sliderService.GetAll().Data;
        List<ImageModel> images = _imageService.GetAll(x => x.SliderId != null).Data;
        ViewBag.images = images;
        return View(sliders);
    }

    public IActionResult AboutUs()
    {
        return View();
    }

    public IActionResult Products()
    {
        var result = _productService.GetAll().Data;
        var imageResult = _imageService.GetAll(null).Data;
        return View(result);
    }

    public IActionResult Contact()
    {
        return View();
    }

    public IActionResult CreateOrder(FormModel formModel)
    {
        var result = _formService.Add(formModel);
        if(result.IsSuccess)
            return RedirectToAction("Contact");

        return BadRequest();
    }
}