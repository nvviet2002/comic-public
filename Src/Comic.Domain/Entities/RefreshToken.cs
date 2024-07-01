using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Comic.Domain.Common;

namespace Comic.Domain.Entities
{
    [Table("RefreshTokens")]
    public class RefreshToken: BaseEntity
    {
        [Required]
        public string UserId { get; set; } = string.Empty;
        [Required]
        public string Token { get; set; } = string.Empty;
        [Required]
        public string JwtId { get; set; } = string.Empty;

        public bool IsRevoked { get; set; } = false;

        public DateTime ExpireAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
    }
}
