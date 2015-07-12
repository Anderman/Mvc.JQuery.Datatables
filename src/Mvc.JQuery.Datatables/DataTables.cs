using System.Linq;
using Mvc.JQuery.Datatables.CSharpModel;

namespace Mvc.JQuery.Datatables
{
    public class DataTables
    {
        public DatatablesRepsonse GetRepsonse<TSource>(IQueryable<TSource> query, DataTablesRequest param)
        {
            var totalRecords = query.Count(); //Execute this query

            var modelProperties = ModelProperties<TSource>.Properties;
            query= DataTablesFiltering.ApplyFilterAndSort(query, modelProperties, param);
            var totalDisplayRecords = query.Count(); //Execute this query

            var skipped = query.Skip(param.Start);
            var data = (param.Length <= 0 ? skipped : skipped.Take(param.Length)).ToArray(); //Execute this query

            var result = new DatatablesRepsonse
            {
                recordsTotal = totalRecords,
                recordsFiltered = totalDisplayRecords,
                draw = param.Draw,
                data = data.Cast<object>().ToArray(),
            };
            return result;
        }
    }
}