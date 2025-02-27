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
// [Authorize(Roles = SD.RoleCompany)]
public class CompanyController : Controller
{
    private readonly IUintOfWork _uintOfWork;
    public CompanyController(IUintOfWork uintOfWork)
    {
        _uintOfWork = uintOfWork;
    }
    // GET
  
    public IActionResult Index()
    {
        List<Company> companies = _uintOfWork.CompanyRepository.GetAll().ToList();
       
        return View(companies);
    }

    public IActionResult Upsert(int? id)
    {
      
        if (id is null or 0)
        {
            //Create
            return View(new Company()
            {
                Name = ""
            });
        }
        else
        {
            //Update
            Company company = _uintOfWork.CompanyRepository.Get(e => e.Id== id);
            return View(company);
        }
    }
    
    [HttpPost]
    public IActionResult Upsert(Company company)
    {
        if (ModelState.IsValid)
        {
                if (company.Id != 0)
                {
                    _uintOfWork.CompanyRepository.Update(company);
                    TempData["Success"] = "company updated";
                }
                else
                {
                     _uintOfWork.CompanyRepository.Add(company);
                     TempData["Success"] = "company Created";
                }

                _uintOfWork.Save();
            return RedirectToAction("Index");
        }
        return View(company);
    }
    
   

    #region MyRegion

    [HttpGet]
    public IActionResult GetAll()
    {
        List<Company> objCompanyList = _uintOfWork.CompanyRepository.GetAll().ToList();
        return Json(new {data = objCompanyList });
    }
    
    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        var companyToDelete = _uintOfWork.CompanyRepository.Get(i=> i.Id == id);
        if (companyToDelete.Id == 0)
        {
            return Json(new { success = false, message = "Company Not Found" });
        }
        _uintOfWork.CompanyRepository.Delete(companyToDelete); 
        _uintOfWork.Save();
        return Json(new {success = true, message = "Company Deleted" });
    }
    #endregion
}