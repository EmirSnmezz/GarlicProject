using System.IO.Compression;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("/Admin")]
public class AdminController : Controller
{
    ISliderService _sliderService;
    IProductService _productService;
    IProductCategoryModelService _productCategoryModel;
    IImageService _imageService;
    IFormService _formService;
    IWebHostEnvironment _env;
    public AdminController(
    ISliderService sliderService,
    IProductService productService,
    IProductCategoryModelService productCategoryModelService,
    IImageService imageService,
    IFormService formService,
    IWebHostEnvironment env)
{
    _sliderService = sliderService;
    _productService = productService;
    _productCategoryModel = productCategoryModelService;
    _imageService = imageService;
    _formService = formService;
    _env = env;
}

    [Route("/Admin/")]
    public IActionResult Index()
    {
        return RedirectToAction(nameof(Slider));
    }

    [HttpGet("Slider")]
    public IActionResult Slider()
    {
        var result = _sliderService.GetAll();
        var imageResult = _imageService.GetAll(x => x.SliderId != null).Data;
        ViewBag.ImageResult = imageResult;
        return View("SliderIndex", result.Data);
    }
    
     [HttpGet("SliderEdit")]
    public IActionResult SliderEdit(string id)
    {
        var result = _sliderService.GetAll().Data.FirstOrDefault(x => x.Id == id);

        var image = _imageService.GetAll(x => x.SliderId == id).Data.FirstOrDefault(x => x.SliderId == id);
        System.Console.WriteLine(image.ImageUrl);
        ViewBag.SliderImage = image;
        return View("SliderEdit", result);
    }

    [HttpGet("AddSlider")]
    public IActionResult AddSlider()
    {
        return View();
    }

    [HttpPost("UpdateSlider")]
    public IActionResult UpdateSlider(SliderModel sliderModel, IFormFile image)
    {
        var entity = _sliderService.GetAll().Data.FirstOrDefault(x => x.Id == sliderModel.Id);

        if (entity == null)
            return NotFound();

        entity.ContentHeader = sliderModel.ContentHeader;
        entity.ContentText = sliderModel.ContentText;

        _sliderService.Update(entity);

        var oldImage = _imageService.GetAll(x => x.SliderId == sliderModel.Id).Data.FirstOrDefault(x => x.SliderId == sliderModel.Id);

        if(oldImage is not null)
            {
                var oldImagePath =  Path.Combine(_env.WebRootPath, oldImage.ImageUrl);    
                if(System.IO.File.Exists(Path.GetFullPath(_env.WebRootPath + oldImage.ImageUrl)))
                {
                    System.IO.File.Delete(Path.Combine(_env.WebRootPath + oldImage.ImageUrl));    
                    var fileName = Guid.NewGuid().ToString() + image.FileName;
                    System.Console.WriteLine(fileName);
                    var path = Path.Combine(_env.WebRootPath, fileName);
                    using var stream = new FileStream(path, FileMode.Create);
                    image.CopyTo(stream);
                    oldImage.ImageUrl = fileName;
                    _imageService.Update(oldImage);
                }
            }
        return RedirectToAction(nameof(Slider));
    }

    [HttpGet("SliderDelete")]
    public IActionResult SliderDelete(string id)
    {
        var entity = _sliderService.GetAll().Data.FirstOrDefault(x=> x.Id == id);
        _sliderService.Remove(entity);

        var sliderImage = _imageService.GetAll(x => x.SliderId == id).Data.FirstOrDefault(x => x.SliderId == id);
        
        if (System.IO.File.Exists(Path.GetFullPath(_env.WebRootPath + sliderImage.ImageUrl)))
        {
            System.IO.File.Delete(Path.Combine(_env.WebRootPath + sliderImage.ImageUrl));
            _imageService.Remove(sliderImage);
        }        

        return RedirectToAction(nameof(Slider));
    }

    [HttpPost("AddSlider")]
    public IActionResult SliderAdd(SliderModel sliderModel, IFormFile imageFile)
    {
        var sliderId = Guid.NewGuid().ToString();
        sliderModel.Id = sliderId;

        _sliderService.Add(sliderModel);

         string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
         var path = Path.Combine(_env.WebRootPath, fileName);
         using var stream = new FileStream(path, FileMode.Create);
         imageFile.CopyTo(stream);

         var image = new ImageModel
         {
             ImageUrl = "/" + fileName,
            SliderId = sliderId,
         };

         _imageService.Add(image);
        
        return RedirectToAction(nameof(Slider));
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


        return View(result);
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
        var path = Path.Combine(_env.WebRootPath, fileName);

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
        
        return RedirectToAction(nameof(Products));
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
        var result = _productService.GetAll(x => x.Id == product.Id).Data.FirstOrDefault(x => x.Id == product.Id);   

        if (result is not null)
        {
            result.ProductDescription = product.ProductDescription;
            result.ProductTitle = product.ProductTitle;

            _productService.Update(result);

            return RedirectToAction(nameof(Products));
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

            ImageModel image = _imageService.GetAll(x => x.ProductId == id).Data.FirstOrDefault(x => x.ProductId == id);

            if(image is not null)
            {
                if (System.IO.File.Exists(Path.GetFullPath(_env.WebRootPath + image.ImageUrl)))
                {
                    System.IO.File.Delete(Path.Combine(_env.WebRootPath + image.ImageUrl));
                    _imageService.Remove(image);
                }     
            }

            return RedirectToAction(nameof(Products));
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
        return RedirectToAction(nameof(Categories));
    }

     [HttpGet("DeleteCategory")]
    public IActionResult DeleteCategory(string id)
    {
        var result = _productCategoryModel.GetAll(x => x.Id == id).Data.FirstOrDefault(x => x.Id == id);
        var deleteResult =_productCategoryModel.Remove(result);

        if(deleteResult.IsSuccess)
        {
            return RedirectToAction(nameof(Categories));
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

            return RedirectToAction(nameof(Categories));
        }

        return RedirectToAction(nameof(Categories));
    }

    [HttpGet("Orders")]
    public IActionResult Orders()
    {
        var result = _formService.GetAll(null).Data;
        return View(result);
    }

    [HttpGet("OrderDetail")]
    public IActionResult OrderDetail(string id)
    {
        var result = _formService.GetAll(x => x.Id == id).Data.FirstOrDefault();

        if(result is not null)
        {
            return View(result);
        }

        return View();
    }

    [HttpGet("ChangeOrderStatus")]
    public IActionResult ChangeOrderStatus(string id)
    {   
        var order = _formService.GetAll(x => x.Id == id).Data.FirstOrDefault(x => x.Id == id);
        _formService.ChangeStatus(order);

        return RedirectToAction(nameof(Orders));
    }

       [HttpGet("Users")]
    public IActionResult Users()
    {
        return View();
    }
}
