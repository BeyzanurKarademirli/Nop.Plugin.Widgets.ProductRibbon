using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.ProductRibbon.Models
{
    /// <summary>
    /// Eklenti yapılandırma sayfasında tanımlı olan ürünlerin listesi
    /// </summary>
    public partial record ProductRibbonProductListModel : BasePagedListModel<ProductRibbonProductModel>
    {
    }
}
