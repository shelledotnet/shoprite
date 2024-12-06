using ShopRite.Application.Dto.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopRite.Application.Dto.Product
{
    public class GetProduct : ProductBase
    {
        public Guid Id { get; set; }

        public GetCategory? Category { get; set; }
    }
}
