using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Bulky.Models;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
namespace Bulky.Areas.Customer.Controllers;

[Area("Customer")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUintOfWork _uintOfWork;
    public HomeController(ILogger<HomeController> logger, IUintOfWork uintOfWork)
    {
        _logger = logger;
        _uintOfWork = uintOfWork;
    }

    public IActionResult Index()
    {
        IEnumerable<Product> products = _uintOfWork.ProductRepository.GetAll(includeproperties: "Category");
        return View(products);
    }
    public IActionResult Details(int productId)
    {
        ShoppingCart shoppingCart = new()
        {
            Product = _uintOfWork.ProductRepository.Get(u => u.Id == productId, includeproperties: "Category"),
            Count = 1,
            ProductId = productId
        };
        return View(shoppingCart);
    }
    [HttpPost]
    [Authorize]
    public IActionResult Details(ShoppingCart shoppingCart)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        shoppingCart.ApplicationUserId = userId;
        
        ShoppingCart CardToAdd =
            (_uintOfWork.ShoppingCartRepository.Get(
                e => (e.ApplicationUserId == shoppingCart.ApplicationUserId) 
                     && (e.ProductId == shoppingCart.ProductId)));
        if (CardToAdd != null)
        {
            CardToAdd.Count += shoppingCart.Count;
            _uintOfWork.ShoppingCartRepository.Update(CardToAdd);
        }
        else
        {
            _uintOfWork.ShoppingCartRepository.Add(shoppingCart);
        }
       TempData["Success"] = "Cart Updated Successfully";
        _uintOfWork.Save();
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}