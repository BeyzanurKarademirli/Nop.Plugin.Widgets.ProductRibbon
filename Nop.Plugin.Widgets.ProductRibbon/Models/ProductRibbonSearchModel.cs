using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.ProductRibbon.Models
{
    public record ProductRibbonSearchModel : BaseSearchModel
    {
        public string SearchText { get; set; }
    }
}
