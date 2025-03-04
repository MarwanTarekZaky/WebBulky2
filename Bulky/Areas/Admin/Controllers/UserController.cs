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
using Microsoft.EntityFrameworkCore;

namespace Bulky.Areas.Admin.Controllers;


[Area("Admin")]
[Authorize(Roles = SD.RoleAdmin)]
public class UserController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IUintOfWork _uintOfWork;
    public UserController(ApplicationDbContext dbContext, IUintOfWork uintOfWork)
    {
        _dbContext = dbContext;
        _uintOfWork = uintOfWork;
    }
    // GET

    public IActionResult Index()
    {
        return View();
    }


    #region MyRegion

    [HttpGet]
    public IActionResult GetAll()
    {
        List<ApplicationUser> objUserList = _dbContext.ApplicationUsers.
            Include(u => u.Company).ToList();
        var rolesTable = _dbContext.Roles;
        foreach (ApplicationUser objUser in objUserList)
        {
            var UserRoleId = _dbContext.UserRoles.
                Where(e => e.UserId == objUser.Id).
                Select(e => e.RoleId).FirstOrDefault();
            
            objUser.Role = _dbContext.Roles.Where(e => e.Id == UserRoleId).Select(e => e.Name).FirstOrDefault();
          
            
            if (objUser.Company is null)
            {
                objUser.Company = new Company()
                {
                    Name = "My Company"
                };
            }
        }
        return Json(new { data = objUserList });
    }

    [HttpPost]
    public IActionResult LockUnlock([FromBody]string id)
    {

        var objFromDb = _uintOfWork.ApplicationUserRepository.Get(u => u.Id == id);
        if (objFromDb == null) 
        {
            return Json(new { success = false, message = "Error while Locking/Unlocking" });
        }

        if(objFromDb.LockoutEnd!=null && objFromDb.LockoutEnd > DateTime.Now) {
            //user is currently locked and we need to unlock them
            objFromDb.LockoutEnd = DateTime.Now;
        }
        else {
            objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
        }
        _uintOfWork.ApplicationUserRepository.Update(objFromDb);
        _uintOfWork.Save();
        return Json(new { success = true, message = "Operation Successful" });
    }


    #endregion

}