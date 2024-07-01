using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Domain.ResponseModels.TokenModel
{
    public class TokenRes
    {
        public string? AccessToken { get; set; }

        public string? RefreshToken { get; set; }
    }
}
