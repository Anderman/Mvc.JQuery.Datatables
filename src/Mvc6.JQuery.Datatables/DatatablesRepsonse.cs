using System;
using System.Linq;

namespace Mvc.JQuery.Datatables
{
    public class DatatablesRepsonse
    {
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public int Draw { get; set; }
        public object[] Data { get; set; }
    }
}
