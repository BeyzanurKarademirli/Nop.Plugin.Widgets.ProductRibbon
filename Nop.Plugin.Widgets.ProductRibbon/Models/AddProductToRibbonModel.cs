using System.Collections.Generic;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.ProductRibbon.Models
{
    /// <summary>
    /// Popup formundan seçilen ürün ID'lerini taşımaya yarayan model
    /// </summary>
    public partial record AddProductToRibbonModel : BaseNopModel
    {
        public AddProductToRibbonModel()
        {
            SelectedProductIds = new List<int>();
        }

        public int RibbonId { get; set; }
        public IList<int> SelectedProductIds { get; set; }
    }
}
