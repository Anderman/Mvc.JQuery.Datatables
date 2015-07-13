using System;
using System.Linq;

namespace Mvc.JQuery.Datatables
{
    public class DatatablesRepsonse
    {
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public int draw { get; set; }
        public object[] data { get; set; }
    }
}
