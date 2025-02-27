using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Bulky.Models.ViewModels;

public class ProductVM
{
    public Product? Product { get; set; }
    [ValidateNever]
    public required IEnumerable<SelectListItem> CategoryList { get; set; }
}
