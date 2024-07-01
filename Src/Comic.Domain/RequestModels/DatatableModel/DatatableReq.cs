using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Domain.RequestModels.DatatableModel
{
    public class DatatableReq
    {
        public List<DatatableColumnData>? Columns { get; set; }

        public int Draw { get; set; }

        public int Length { get; set; }

        public int Start { get; set; }

        public List<DatatableOrder>? Order { get; set; }

        public DatatableSearch? Search { get; set; }
    }
    public class DatatableColumnData
    {
        public string? Name { set; get; }
        public string? Data { set; get; }
        public bool? Searchable { set; get; }
        public bool? Orderable { set; get; }
    }

    public class DatatableOrder
    {
        public string? Column { set; get; }
        public string? Dir { set; get; }
    }

    public class DatatableSearch
    {
        public string? Value { set; get; }
        public bool? Regex { set; get; }
    }
}
