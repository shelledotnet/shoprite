using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopRite.Domain.Entities
{
    public class Product
    {
        #region Scalar Navigation Property
        [Key]
        public Guid Id { get; set; }

        public string? Name { get; set; }
        public string? Description { get; set; }

        public decimal Price { get; set; }
        public string? Image { get; set; }
        public int Quantity { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedDate { get; set; }
        #endregion

        #region Collection Navigation Property
        public Category? Category { get; set; }

        public Guid CategoryId { get; set; } 
        #endregion
    }
}
