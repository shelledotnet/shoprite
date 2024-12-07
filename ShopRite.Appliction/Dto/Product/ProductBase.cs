using System.ComponentModel.DataAnnotations;

namespace ShopRite.Application.Dto.Product
{
    public class ProductBase
    {
        [Required(ErrorMessage = "{0} is required")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "invalid {0} ")]
        [StringLength(50, ErrorMessage = "{0} max length is 50"), MinLength(3, ErrorMessage = "{0} min length is 3")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "invalid {0} ")]
        [StringLength(50, ErrorMessage = "{0} max length is 50"), MinLength(3, ErrorMessage = "{0} min length is 3")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "{0} is required")]

        public string? Base64Image { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public Guid CategoryId { get; set; }
    }
}
