using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.ProductRibbon.Models
{
    /// <summary>
    /// Rozete bağlı veya popup gridinde gösterilecek ürün modeli
    /// </summary>
    public partial record ProductRibbonProductModel : BaseNopEntityModel
    {
        public int RibbonId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public bool Published { get; set; }
    }
}
