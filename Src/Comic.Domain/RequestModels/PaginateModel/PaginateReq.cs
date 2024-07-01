using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Domain.RequestModels.PaginateModel
{
    public class PaginateReq
    {
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
        public string? SearchTerm { get; set; } = string.Empty;

    }
}
