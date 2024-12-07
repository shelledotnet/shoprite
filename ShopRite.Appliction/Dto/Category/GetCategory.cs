// Ignore Spelling: Dto

using ShopRite.Application.Dto.Product;
using System.ComponentModel.DataAnnotations;

namespace ShopRite.Application.Dto.Category
{
    public class GetCategory : CategoryBase
    {
        [Required(ErrorMessage = "{0} is required")]
        public Guid Id { get; set; }

       public ICollection<GetProduct>? Products { get; set; }

    }

    
}
