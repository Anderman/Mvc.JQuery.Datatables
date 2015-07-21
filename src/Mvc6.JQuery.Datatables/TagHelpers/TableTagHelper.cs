using System.Threading.Tasks;
using Microsoft.AspNet.Razor.Runtime.TagHelpers;

namespace Mvc.JQuery.Datatables.TagHelpers
{
    [TargetElement("table", Attributes = "asp-datatables-language")]
    [TargetElement("table", Attributes = "asp-datatables-lengthmenu")]
    [TargetElement("table", Attributes = "asp-datatables-savestate")]
    [TargetElement("table", Attributes = "asp-datatables-url")]
    
    public class TableTagHelper : TagHelper
    {
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