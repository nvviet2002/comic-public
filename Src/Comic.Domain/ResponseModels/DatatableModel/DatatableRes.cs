using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Comic.Domain.ResponseModels.DatatableModel
{
    public class DatatableRes<T>
    {
        public int Draw { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public ICollection<T> Data { get; set; }
    }
}
