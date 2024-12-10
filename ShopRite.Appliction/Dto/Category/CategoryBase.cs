using System.ComponentModel.DataAnnotations;

namespace ShopRite.Application.Dto.Category
{
    public class CategoryBase
    {
        [Required(ErrorMessage = "{0} is required")]
        [RegularExpression(@"^[-0-9a-zA-Z.,\s]+$", ErrorMessage = "invalid {0} ")]
        [StringLength(30, ErrorMessage = "{0} max Length is 30"), MinLength(3, ErrorMessage = "{0} must be at least 3 characters long")]
        public string? Name { get; set; }

    }
}
