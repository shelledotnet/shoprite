// Ignore Spelling: Dto

using System.ComponentModel.DataAnnotations;

namespace ShopRite.Application.Dto.Category
{
    public class UpdateCategory : CategoryBase
    {
        [Required(ErrorMessage = "{0} is required")]
        public Guid Id { get; set; }
    }
}
