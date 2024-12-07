using System.ComponentModel.DataAnnotations;

namespace ShopRite.Application.Dto.Category
{
    public class CategoryBase
    {
        [Required(ErrorMessage = "{0} is required")]
        public string? Name { get; set; }

    }
}
