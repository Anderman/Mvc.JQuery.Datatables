using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Razor.Runtime.TagHelpers;
using Anderman.TagHelpers;
namespace Mvc.JQuery.Datatables.TagHelpers
{
    [HtmlTargetElement("th", Attributes = "asp-datatables-data")]
    [HtmlTargetElement("th", Attributes = "asp-datatables-render")]
    [HtmlTargetElement("th", Attributes = "asp-datatables-render-arg")]
    [HtmlTargetElement("th", Attributes = "asp-datatables-orderable")]
    [HtmlTargetElement("th", Attributes = "asp-datatables-searchable")]
    public class ThTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-datatables-data")]
        public ModelExpression For { get; set; }
        [HtmlAttributeName("asp-datatables-render")]
        public string Renderfunction { get; set; }
        [HtmlAttributeName("asp-datatables-render-arg")]
        public string RenderArg { get; set; }
        [HtmlAttributeName("asp-datatables-orderable")]
        public bool? Orderable { get; set; }
        [HtmlAttributeName("asp-datatables-searchable")]
        public bool? Searchable { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (For != null)
            {
                output.Attributes.Add("data-data", For.Name);
                output.Content.Append(For?.Metadata.GetShortName() ?? For?.Metadata.GetDisplayName());
            }
            if (Renderfunction != null)
                output.Attributes.Add("data-render", Renderfunction);
            if (RenderArg != null)
                output.Attributes.Add("data-render-arg", RenderArg);
            if (Orderable != null)
                output.Attributes.Add("data-orderable", Orderable.ToString().ToLower());
            if (Searchable != null)
                output.Attributes.Add("data-searchable", Searchable.ToString().ToLower());
            return Task.FromResult(0);
        }
    }
}