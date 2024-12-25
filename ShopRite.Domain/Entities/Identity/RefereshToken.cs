using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopRite.Domain.Entities.Identity
{
    public class RefereshToken
    {
        [Key]
        public int Id { get; set; }
        public string UsersId { get; set; } = string.Empty;
        public string JwtId { get; set; } = string.Empty;   //jwt id of the refereshToken
        public string Token { get; set; } = string.Empty;//refereshToken

        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateExpired { get; set; }
    }
}
