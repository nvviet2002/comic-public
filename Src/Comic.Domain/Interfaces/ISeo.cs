using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Domain
{
    public interface ISeo
    {
        string Slug { get; set; }
        string MetaKeyword { get; set; }
        string MetaDescription { get; set; }

    }
}
