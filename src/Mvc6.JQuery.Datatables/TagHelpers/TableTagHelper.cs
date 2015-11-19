using System.Threading.Tasks;
using Microsoft.AspNet.Razor.Runtime.TagHelpers;
using Microsoft.AspNet.Razor.TagHelpers;

namespace Mvc.JQuery.Datatables.TagHelpers
{
    [HtmlTargetElement("table", Attributes = "asp-datatables-language")]
    [HtmlTargetElement("table", Attributes = "asp-datatables-lengthmenu")]
    [HtmlTargetElement("table", Attributes = "asp-datatables-savestate")]
    [HtmlTargetElement("table", Attributes = "asp-datatables-url")]
    
    public class TableTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-datatables-edit")]
        public string DatatablesEdit { get; set; }

        [HtmlAttributeName("asp-datatables-delete")]
        public string DatatablesDelete { get; set; }

        [HtmlAttributeName("asp-datatables-create")]
        public string DatatablesCreate { get; set; }

        [HtmlAttributeName("asp-datatables-edit-row-select")]
        public string DatatablesEditRowSelect { get; set; }

        [HtmlAttributeName("asp-datatables-edit-button")]
        public string DatatablesEditButton { get; set; }

        [HtmlAttributeName("asp-datatables-delete-button")]
        public string DatatablesDeleteButton { get; set; }

        [HtmlAttributeName("asp-datatables-create-button")]
        public string DatatablesCreateButton { get; set; }

        [HtmlAttributeName("asp-datatables-language")]
        public string DatatablesLang { get; set; }
        [HtmlAttributeName("asp-datatables-lengthmenu")]
        public string Lengthmenu { get; set; }
        [HtmlAttributeName("asp-datatables-savestate")]
        public bool? Savestate { get; set; }
        [HtmlAttributeName("asp-datatables-url")]
        public string Url { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (!string.IsNullOrEmpty(DatatablesEdit))
                output.Attributes.Add("data-edit", DatatablesEdit);
            if (!string.IsNullOrEmpty(DatatablesDelete))
                output.Attributes.Add("data-delete", DatatablesDelete);
            if (!string.IsNullOrEmpty(DatatablesCreate))
                output.Attributes.Add("data-create", DatatablesCreate);

            if (!string.IsNullOrEmpty(DatatablesEditRowSelect))
                output.Attributes.Add("data-edit-row-select", DatatablesEditRowSelect);

            if (!string.IsNullOrEmpty(DatatablesEditButton))
                output.Attributes.Add("data-edit-button", DatatablesEditButton);
            if (!string.IsNullOrEmpty(DatatablesDeleteButton))
                output.Attributes.Add("data-delete-button", DatatablesDeleteButton);
            if (!string.IsNullOrEmpty(DatatablesCreateButton))
                output.Attributes.Add("data-create-button", DatatablesCreateButton);



            if (!string.IsNullOrEmpty(DatatablesLang))
                output.Attributes.Add("data-language", DatatablesLang);
            if (!string.IsNullOrEmpty(Lengthmenu))
                output.Attributes.Add("data-lengthmenu", Lengthmenu);
            if (!Savestate != null)
                output.Attributes.Add("data-savestate", Savestate);
            if (!string.IsNullOrEmpty(Url))
                output.Attributes.Add("data-url", Url);
            return Task.FromResult(0);
        }
    }
}