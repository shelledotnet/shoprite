// Ignore Spelling: Dto

using ShopRite.Application.Dto.Product;

namespace ShopRite.Application.Dto.Category
{
    public class GetCategory : CategoryBase
    {
        public Guid Id { get; set; }

       public ICollection<GetProduct>? Products { get; set; }

    }

    
}
