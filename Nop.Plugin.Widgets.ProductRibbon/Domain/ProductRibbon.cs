using Nop.Core;

namespace Nop.Plugin.Widgets.ProductRibbon.Domain
{
    /// <summary>
    /// Ürün Etiket/Rozet (Ribbon) tanımlamalarını tutan Entity sınıfı.
    /// </summary>
    public class ProductRibbon : BaseEntity
    {
        /// <summary>
        /// Etiket metni (Örn: "Yeni", "Çok Satan", "%20 İndirim", "Tükendi")
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Etiket arka plan rengi (HEX / CSS kodu: #ef4444 vb.)
        /// </summary>
        public string BackgroundColor { get; set; }

        /// <summary>
        /// Etiket yazı rengi (HEX / CSS kodu: #ffffff vb.)
        /// </summary>
        public string TextColor { get; set; }

        /// <summary>
        /// Etiketin aktif veya pasif olma durumu
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gösterim sırası
        /// </summary>
        public int DisplayOrder { get; set; }
    }
}
