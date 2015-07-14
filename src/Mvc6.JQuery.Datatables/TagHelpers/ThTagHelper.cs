using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Razor.Runtime.TagHelpers;

namespace Mvc.JQuery.Datatables.TagHelpers
{
    [TargetElement("th", Attributes = "asp-datatables-data")]
    [TargetElement("th", Attributes = "asp-datatables-render")]
    [TargetElement("th", Attributes = "asp-datatables-render-arg")]
    [TargetElement("th", Attributes = "asp-datatables-orderable")]
    [TargetElement("th", Attributes = "asp-datatables-searchable")]
    public class thTagHelper : TagHelper
    {
        /// <summary>
        /// test sum
        /// </summary>
        [HtmlAttributeName("asp-datatables-data")]
        public ModelExpression For { get; set; }
        [HtmlAttributeName("asp-datatables-render")]
        public string Renderfunction { get; set; }
        [HtmlAttributeName("asp-datatables-render-arg")]
        public string renderArg { get; set; }
        [HtmlAttributeName("asp-datatables-orderable")]
        public bool? orderable { get; set; }
        [HtmlAttributeName("asp-datatables-searchable")]
        public bool? searchable { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (For != null)
            {
                output.Attributes.Add("data-data", For.Name);
                output.Content.Append(For?.Metadata?.DisplayName ?? For.Name);
            }
            if(Renderfunction!=null)
                output.Attributes.Add("data-render", Renderfunction);
            if (renderArg != null)
                output.Attributes.Add("data-render-arg", renderArg);
            if (orderable != null)
                output.Attributes.Add("data-orderable", orderable);
            if (searchable != null)
                output.Attributes.Add("data-searchable", searchable);
            return Task.FromResult(0);
        }
    }
}