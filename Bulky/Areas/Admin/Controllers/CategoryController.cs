using Bulky.DataAccess.Data;
using Bulky.DataAccess.Migrations;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bulky.Areas.Admin.Controllers;


[Area("Admin")]
// [Authorize(Roles = SD.RoleAdmin)]
public class CategoryController : Controller
{
    private readonly IUintOfWork _uintOfWork;
    
    public CategoryController(IUintOfWork uintOfWork)
    {
        _uintOfWork = uintOfWork;
    }
    // GET
    public IActionResult Index()
    {
        List<Category> categories = _uintOfWork.CategoryRepository.GetAll().ToList();
        return View(categories);
    }

    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Create(Category category)
    {
        if (category.Name == category.DisplayOrder.ToString())
        {
            ModelState.AddModelError("DisplayOrder", "Display Can not exactly equals Name ");
        }
        if (ModelState.IsValid)
        {
           _uintOfWork.CategoryRepository.Add(category);
            _uintOfWork.Save();
            TempData["Success"] = "Category Created";
            return RedirectToAction("Index", "Category");
        }

        return View(category);
    }
    
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        Category? category = _uintOfWork.CategoryRepository.Get(i=> i.Id == id);
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }
    
    [HttpPost]
    public IActionResult Edit(Category category)
    {
        
        if (ModelState.IsValid)
        {
           _uintOfWork.CategoryRepository.Update(category);
            _uintOfWork.Save();
            TempData["Success"] = "Category Updated";
            return RedirectToAction("Index", "Category");
        }

        return View(category);
    }
    
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        Category? category = _uintOfWork.CategoryRepository.Get(i=> i.Id == id);
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }
    
    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePost(int? id)
    {
        Category? category = _uintOfWork.CategoryRepository.Get(i=> i.Id == id);
        if (category == null)
        {
            return NotFound();
        }
        else{
            
            _uintOfWork.CategoryRepository.Delete(category);
            _uintOfWork.Save();
            TempData["Success"] = "Category Deleted";
            return RedirectToAction("Index", "Category");
        }
    }
    
}