using Bulky.DataAccess.Data;
using Bulky.DataAccess.Migrations;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.DataAccess.Repository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bulky.Areas.Admin.Controllers;


[Area("Admin")]
// [Authorize(Roles = SD.RoleAdmin)]
public class ProductController : Controller
{
    private readonly IUintOfWork _uintOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment; 
    public ProductController(IUintOfWork uintOfWork, IWebHostEnvironment webHostEnvironment)
    {
        _uintOfWork = uintOfWork;
        _webHostEnvironment = webHostEnvironment;
    }
    // GET
  
    public IActionResult Index()
    {
        List<Product> products = _uintOfWork.ProductRepository.GetAll(includeproperties:"Category").ToList();
       
        return View(products);
    }

    public IActionResult Upsert(int? id)
    {
        ProductVM productVM = new()
        {
            CategoryList = _uintOfWork.CategoryRepository.GetAll().Select(e => new SelectListItem {
            Text = e.Name,
            Value = e.Id.ToString() }),
            Product = new Product
            {
                Title = null,
                ISBN = null,
                Author = null,
                ListPrice = 0,
                Price = 0,
                Price50 = 0,
                Price100 = 0,
                CategoryId = 0,
                Description = null
            }
        };
        if (id is null or 0)
        {
            ///Create
            return View(productVM);
        }
        else
        {
            //Update
            productVM.Product = _uintOfWork.ProductRepository.Get(e => e.Id== id);
            return View(productVM);
        }
    }
    
    [HttpPost]
    public IActionResult Upsert(ProductVM productvm, IFormFile? file )
    {
        if (ModelState.IsValid)
        {
            if (file != null)
            { 
                string wwwrootPath = _webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string productpath = Path.Combine(wwwrootPath, @"images/product");
              
                    if (productvm.Product.IamgeURL != null)
                    {
                        var oldImagePath = Path.Combine(wwwrootPath,
                            productvm.Product.IamgeURL?.TrimStart('/') ?? string.Empty);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            //Removing the old Image
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var stream = new FileStream(Path.Combine(productpath, fileName), FileMode.Create))
                    {
                        file.CopyTo(stream);

                    }
                    productvm.Product.IamgeURL = @"/images/product/" + fileName;

                if (productvm.Product.Id == 0)
                {
                    //Creating  new product
                    _uintOfWork.ProductRepository.Add(productvm.Product);
                    TempData["Success"] = "Product Created Successfully";
                }
                else
                {
                    //Editing Product with new Image
                    _uintOfWork.ProductRepository.Update(productvm.Product);
                    TempData["Success"] = "Product Updated Successfully";
                }
                
            }
            else
            {
                //Editing The ProductVM without image
                if (productvm.Product != null && productvm.Product.Id != 0)
                {
                    _uintOfWork.ProductRepository.Update(productvm.Product);
                    TempData["Success"] = "Product Updated Successfully";
                }
            }
            _uintOfWork.Save();
            return RedirectToAction("Index", "Product");
        }
        else
        {
            //Not a Valid Model
            TempData["Success"] = "Product data is Invalid !! =)";
            return View();
        }
        
    }
    
   

    #region MyRegion

    [HttpGet]
    public IActionResult GetAll()
    {
        List<Product> objProductList = _uintOfWork.ProductRepository.GetAll(includeproperties: "Category").ToList();
        return Json(new {data = objProductList });
    }
    
    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        var productToDelete = _uintOfWork.ProductRepository.Get(i=> i.Id == id);
        if (productToDelete == null)
        {
            return Json(new { success = false, message = "Product Not Found" });
        }
        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productToDelete.
            IamgeURL?.TrimStart('/') ?? string.Empty);
        if (System.IO.File.Exists(oldImagePath))
        {
            System.IO.File.Delete(oldImagePath);
        }
        _uintOfWork.ProductRepository.Delete(productToDelete); 
        _uintOfWork.Save();
        return Json(new {success = true, message = "Product Deleted" });
    }

    #endregion
}