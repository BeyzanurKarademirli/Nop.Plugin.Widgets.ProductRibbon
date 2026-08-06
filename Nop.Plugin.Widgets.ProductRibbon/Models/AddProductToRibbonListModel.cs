using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.ProductRibbon.Models
{
    /// <summary>
    /// Popup içerisindeki sayfa sergilemeli ürün listesi modeli
    /// </summary>
    public partial record AddProductToRibbonListModel : BasePagedListModel<ProductRibbonProductModel>
    {
    }
}
