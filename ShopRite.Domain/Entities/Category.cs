using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopRite.Domain.Entities
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public ICollection<Product>? Products { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedDate { get; set; }
    }
}
