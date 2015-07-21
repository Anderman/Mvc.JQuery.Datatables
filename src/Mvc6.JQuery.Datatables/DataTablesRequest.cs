using System;
using System.Collections.Generic;

namespace Mvc.JQuery.Datatables
{
    public class DataTablesRequest
    {
        public int Start { get; set; }
        public int Length { get; set; }
        public int Draw { get; set; }
        public SearchT Search { get; set; }

        public List<OrderT> Order { get; set; }

        public ColumnT[] Columns { get; set; }
        public class OrderT
        {
            public int Column;
            public string Dir;
        }

        public class ColumnT
        {
            public string Data;
            public string Name;
            public Boolean Searchable;
            public Boolean Orderable;
            public SearchT Search;
        }
        public class SearchT
        {
            public string Value;
            public Boolean Regex;
        }
        //public RequestTypes RequestType;

        ///// <summary>
        ///// Request type values
        ///// </summary>
        //public enum RequestTypes
        //{
        //    /// <summary>
        //    /// DataTables standard get for client-side processing
        //    /// </summary>
        //    DataTablesGet,

        //    /// <summary>
        //    /// DataTables server-side processing request
        //    /// </summary>
        //    DataTablesSsp,

        //    /// <summary>
        //    /// Editor create request
        //    /// </summary>
        //    EditorCreate,

        //    /// <summary>
        //    /// Editor edit request
        //    /// </summary>
        //    EditorEdit,

        //    /// <summary>
        //    /// Editor remove request
        //    /// </summary>
        //    EditorRemove,

        //    /// <summary>
        //    /// Editor file upload request
        //    /// </summary>
        //    EditorUpload
        //};





        /////** Editor parameters **/

        //public string Action;

        //public Dictionary<string, object> Data;

        //public List<string> Ids = new List<string>();

        //public string UploadField;

    }
}
