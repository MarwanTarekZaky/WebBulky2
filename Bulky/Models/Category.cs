using System.ComponentModel.DataAnnotations;

namespace Bulky.Models;

public class Category
{
    [Key]
    public int Id {
        get;
        set;
    }

    [Display(Name = "Name")]
    [Required]
    public string Name { get; set; }
    
    
    [Display(Name = "Display Order")]
    public int DisplayOrder { get; set; }
}