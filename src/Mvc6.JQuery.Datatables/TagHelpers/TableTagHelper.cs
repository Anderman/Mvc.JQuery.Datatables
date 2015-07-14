using System.Threading.Tasks;
using Microsoft.AspNet.Razor.Runtime.TagHelpers;

namespace Mvc.JQuery.Datatables.TagHelpers
{
    [TargetElement("table", Attributes = "asp-datatables-language")]
    [TargetElement("table", Attributes = "asp-datatables-lengthmenu")]
    [TargetElement("table", Attributes = "asp-datatables-savestate")]
    [TargetElement("table", Attributes = "asp-datatables-url")]
    
    public class tableTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-datatables-language")]
        public string datatablesLang { get; set; }
        [HtmlAttributeName("asp-datatables-lengthmenu")]
        public string lengthmenu { get; set; }
        [HtmlAttributeName("asp-datatables-savestate")]
        public bool? savestate { get; set; }
        [HtmlAttributeName("asp-datatables-url")]
        public string url { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (!string.IsNullOrEmpty(datatablesLang))
                output.Attributes.Add("data-language", datatablesLang);
            if (!string.IsNullOrEmpty(lengthmenu))
                output.Attributes.Add("data-lengthmenu", lengthmenu);
            if (!savestate != null)
                output.Attributes.Add("data-savestate", savestate);
            if (!string.IsNullOrEmpty(url))
                output.Attributes.Add("data-url", url);
            return Task.FromResult(0);
        }
    }
}