using Nop.Core;

namespace Nop.Plugin.Widgets.ProductRibbon.Domain
{
    /// <summary>
    /// Ürünler ile Etiketler arasındaki bire-çok / çoka-çok eşleşmeleri tutan Entity sınıfı.
    /// </summary>
    public class ProductRibbonMapping : BaseEntity
    {
        /// <summary>
        /// Eşleştirilen Ürünün ID değeri
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Eşleştirilen Etiketin (ProductRibbon) ID değeri
        /// </summary>
        public int ProductRibbonId { get; set; }
    }
}
