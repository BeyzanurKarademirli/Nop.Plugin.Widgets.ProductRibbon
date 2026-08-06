using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.ProductRibbon.Models
{
    /// <summary>
    /// Eklenti yapılandırma sayfasında tanımlı ürünleri listelemek için arama modeli
    /// </summary>
    public partial record ProductRibbonProductSearchModel : BaseSearchModel
    {
        public int ProductRibbonId { get; set; }
    }
}
