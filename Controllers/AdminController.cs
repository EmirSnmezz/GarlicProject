using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("/Admin")]
public class AdminController : Controller
{
    ISliderService _sliderService;
    IProductService _productService;
    IProductCategoryModelService _productCategoryModel;
    IImageService _imageService;
    public AdminController(ISliderService sliderService, IProductService productService, IProductCategoryModelService productCategoryModelService,IImageService imageService)
    {
        _sliderService = sliderService;
        _productService = productService;
        _productCategoryModel = productCategoryModelService;
        _imageService = imageService;
    }

    [Route("/Admin/")]
    public IActionResult Index()
    {
        return RedirectToAction("Slider");
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

    [HttpGet("SliderDelete")]
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

        List<ImageModel> images = new List<ImageModel>();

        foreach(var product in result)
        {
            images = _imageService.GetAll(x => x.ProductId == product.Id).Data;
        }

        ViewBag.Images = images;

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
        var path = Path.Combine("wwwroot", fileName);

        using var stream = new FileStream(path, FileMode.Create);
        imageFile.CopyTo(stream);

        product.Id = Guid.NewGuid().ToString();

        var image = new ImageModel
        {
            ImageUrl = "/" + fileName,
            DisplayPriority = 1,
            ProductId = product.Id
        };
        var result = _imageService.Add(image);

        if(result.IsSuccess)
        {
         product.ImageId = image.Id;
        _productService.Add(product);   
        
        return RedirectToAction("Products");
        }

        return BadRequest();
    }

    [HttpGet("UpdateProduct")]
    public IActionResult UpdateProduct(string id)
    {
        var result = _productService.GetById(x => x.Id == id).Data;

        var categories = _productCategoryModel.GetAll().Data.ToList();
        ViewBag.Categories = categories;

        if(result is not null)
        {
            return View("UpdateProduct",result);
        }

        return NotFound();
    }

     [HttpPost("UpdateProduct")]
    public IActionResult UpdateProduct(ProductModel product)
    {
        var result = _productService.GetAll(x => x.Id == product.Id).Data.FirstOrDefault();   

        if (result is not null)
        {
            result.ProductDescription = product.ProductDescription;
            result.ProductTitle = product.ProductTitle;

            _productService.Update(result);

            return RedirectToAction("Products");
        }
        
        return NotFound();
    }

    [HttpGet("ProductDelete")]
    public IActionResult ProductDelete(string id)
    {
        var result = _productService.GetAll(x => x.Id == id).Data.FirstOrDefault();

        if(result is not null)
        {
            _productService.Remove(result);

            return RedirectToAction("Products");
        }

        return NotFound();
    }

    [HttpGet("Categories")]
    public IActionResult Categories()
    {
        var result = _productCategoryModel.GetAll().Data;
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

     [HttpGet("DeleteCategory")]
    public IActionResult DeleteCategory(string id)
    {
        var result = _productCategoryModel.GetAll(x => x.Id == id).Data.FirstOrDefault(x => x.Id == id);
        var deleteResult =_productCategoryModel.Remove(result);

        if(deleteResult.IsSuccess)
        {
            return RedirectToAction("Categories");
        }

        return BadRequest();
    }

   [HttpGet("EditCategory")]
    public IActionResult EditCategory(string id)
    {
        var result = _productCategoryModel.GetAll(x => x.Id == id).Data.FirstOrDefault();
        return View("EditCategory", result);
    }

    [HttpPost("EditCategory")]
    public IActionResult CategoryEdit(ProductCategoryModel category)
    {
        var result = _productCategoryModel.GetAll(x => x.Id == category.Id).Data.FirstOrDefault();

        if(result is not null)
        {
             result.Name = category.Name;
            _productCategoryModel.Update(result);

            return RedirectToAction("Categories");
        }

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
