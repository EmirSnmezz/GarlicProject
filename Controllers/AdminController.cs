using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("/Admin")]
public class AdminController : Controller
{
    ISliderService _sliderService;
    IProductService _productService;
    IProductCategoryModelService _productCategoryModel;
    public AdminController(ISliderService sliderService, IProductService productService, IProductCategoryModelService productCategoryModelService)
    {
        _sliderService = sliderService;
        _productService = productService;
        _productCategoryModel = productCategoryModelService;
    }

    [Route("/Admin/")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("Slider")]
    public IActionResult Slider()
    {
        var result = _sliderService.GetAll();
        return View("SliderIndex", result.Data);
    }
    
     [HttpGet("SliderEdit")]
    public IActionResult SliderEdit(string id)
    {
        var result = _sliderService.GetAll().Data.FirstOrDefault(x => x.Id == id);
        return View("SliderEdit", result);
    }

    [HttpGet("AddSlider")]
    public IActionResult AddSlider()
    {
        return View();
    }

    [HttpPost("UpdateSlider")]
    public IActionResult UpdateSlider(SliderModel sliderModel)
    {
        var entity = _sliderService.GetAll().Data.FirstOrDefault();

    if (entity == null)
        return NotFound();

    entity.ContentHeader = sliderModel.ContentHeader;
    entity.ContentText = sliderModel.ContentText;

    _sliderService.Update(entity);

    return RedirectToAction("Slider");
    }

    [HttpDelete]
    public IActionResult SliderDelete(string id)
    {
        var entity = _sliderService.GetAll().Data.FirstOrDefault(x=> x.Id == id);
        _sliderService.Remove(entity);

        return RedirectToAction("Slider");
    }

    [HttpPost("AddSlider")]
    public IActionResult SliderAdd(SliderModel sliderModel)
    {
        _sliderService.Add(sliderModel);
        return RedirectToAction("Slider");
    }

    [HttpGet("Products")]
    public IActionResult Products()
    {
        var result = _productService.GetAll().Data;

        return View("ProductIndex", result);
    }

    [HttpGet("AddProduct")]
    public IActionResult AddProduct()
    {
        var categories = _productCategoryModel.GetAll().Data.ToList();
        ViewBag.Categories = categories;
        return View("ProductsAdd");
    }

    [HttpPost("AddProduct")]
    public IActionResult ProductAdd(ProductModel product, IFormFile imageFile)
    {
        var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
    var path = Path.Combine("wwwroot/images", fileName);

    using var stream = new FileStream(path, FileMode.Create);
    imageFile.CopyTo(stream);

    // 2️⃣ Image DB kaydı
    var image = new ImageModel
    {
        ImageUrl = "/images/" + fileName,
        DisplayPriority = 1,
        SliderId = "32131"
    };

    //_context.SliderImages.Add(image);
         product.ImageId = image.Id;
        _productService.Add(product);

        return RedirectToAction("ProductIndex");
    }

    [HttpGet("Categories")]
    public IActionResult Categories()
    {
        var result = _productCategoryModel.GetAll().Data;
        System.Console.WriteLine( $"Data: {result.FirstOrDefault()}" );
        return View("Categories", result);
    }

    [HttpGet("AddCategory")]
    public IActionResult AddCategory()
    {
        return View();
    }

    [HttpPost("AddCategory")]
    public IActionResult AddCategory(ProductCategoryModel category)
    {
        _productCategoryModel.Add(category);
        return RedirectToAction("Categories");
    }

    [HttpGet("Orders")]
    public IActionResult Orders()
    {
        return View();
    }

       [HttpGet("Users")]
    public IActionResult Users()
    {
        return View();
    }
}
