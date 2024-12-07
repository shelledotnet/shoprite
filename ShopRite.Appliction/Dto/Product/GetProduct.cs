using ShopRite.Application.Dto.Category;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopRite.Application.Dto.Product
{
    public class GetProduct : ProductBase
    {
        [Required(ErrorMessage = "{0} is required")]
        public Guid Id { get; set; }

        public GetCategory? Category { get; set; }
    }
}
