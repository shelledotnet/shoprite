using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopRite.Application
{
    public class ProductById
    {
        [FromRoute]
        //this is a value type you dont need required attribute it will always have a min value
        //[Required(ErrorMessage = "{0} is required")]
        [DefaultValue(typeof(Guid), "8DAEDD83-289E-417C-AAAC-D48D77E0D84C")]
        public Guid Id { get; set; }
    }
}
