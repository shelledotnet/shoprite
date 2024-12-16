using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopRite.Application.Services
{
    public class ProjectOptions
    {
        [Required(AllowEmptyStrings = false)]
        public string? SecreteKey { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string? ValidIssuer { get; set; }

        [Required(AllowEmptyStrings = false)]
        public TimeSpan TokenLifeTime { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string[]? ValidAudiences { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string? XApiKeyMap { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string? XApiKey { get; set; }
    }
}
