using System.ComponentModel.DataAnnotations;

namespace Bulky.Models;

public class Product
{
    [Key]
    public int Id { get; set; }
    public required string Title { get; set; }
    public string Description { get; set; }
    public required string ISBN { get; set; }
    public required string Author { get; set; }
    [Range(1,1000)]
    [Display(Name = "Price for Unit")]
    public required decimal ListPrice { get; set; }
    [Range(1,1000)]
    [Display(Name = "Price for 1-50 Unit")]
    public required decimal Price { get; set; }
    [Range(1,1000)]
    [Display(Name = "Price per 50+ Unit")]
    public required decimal Price50 { get; set; }
    [Range(1,1000)]
    [Display(Name = "Price per 100+ Unit")]
    public required decimal Price100 { get; set; }
    
    public required int CategoryId { get; set; }
    
    
}