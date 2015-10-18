using System.Linq;
using Mvc.JQuery.Datatables.CSharpModel;
using Microsoft.AspNet.Mvc;

namespace Mvc.JQuery.Datatables
{
    public class DataTables
    {
        public DatatablesRepsonse GetResponse<TSource>(IQueryable<TSource> query, DataTablesRequest param)
        {
            var totalRecords = query.Count(); //Execute this query

            var modelProperties = ModelProperties<TSource>.Properties;
            query = DataTablesFiltering.ApplyFilterAndSort(query, modelProperties, param);
            var totalDisplayRecords = query.Count(); //Execute this query

            var skipped = query.Skip(param.Start);
            var data = (param.Length <= 0 ? skipped : skipped.Take(param.Length)).ToArray(); //Execute this query

            return new DatatablesRepsonse
            {
                recordsTotal = totalRecords,
                recordsFiltered = totalDisplayRecords,
                draw = param.Draw,
                data = data.Cast<object>().ToArray(),
            };
        }

        public JsonResult GetJSonResult<TSource>(IQueryable<TSource> query, DataTablesRequest param)
        {
            var result = GetResponse(query, param);
            return new JsonResult(result);
        }
    }
}