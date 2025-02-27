using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Bulky.DataAccess;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;

namespace Bulky.Areas.Customer.Controllers;

[Area("Customer")]
[Authorize]
public class CartController : Controller
{
    // GET
    private readonly IUintOfWork _unitOfWork;
    public ShoppingCartVM ShoppingCartVM { get; set; }

    public CartController(IUintOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
      
    }
    public IActionResult Index()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        ShoppingCartVM = new ()
        {
            ShoppingCartList = _unitOfWork.ShoppingCartRepository.GetAll(e => e.ApplicationUserId
            == userId, includeproperties:"Product"),
        };
        return View();
    }
}