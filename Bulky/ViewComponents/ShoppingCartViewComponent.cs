using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Utility;

namespace Bulky.ViewComponents;

public class ShoppingCartViewComponent: ViewComponent
{
    private readonly IUintOfWork _uintOfWork;
    public ShoppingCartViewComponent(IUintOfWork uintOfWork)
    {
        _uintOfWork = uintOfWork;
       
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        if (claim != null)
        {
            if (HttpContext.Session.GetInt32(SD.SessionCart) == null)
            {
                HttpContext.Session.SetInt32(SD.SessionCart,
                    _uintOfWork.ShoppingCartRepository.GetAll(u => u.ApplicationUserId == claim.Value).Count());
            }
            
            return View(HttpContext.Session.GetInt32(SD.SessionCart));
        }
        else
        {
            HttpContext.Session.Clear();
            return View(0);
        }
    }
}