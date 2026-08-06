using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widgets.ProductRibbon.Models
{
    public record ProductRibbonModel : BaseNopEntityModel
    {
        public ProductRibbonModel()
        {
            ProductSearchModel = new ProductRibbonProductSearchModel();
        }

        [NopResourceDisplayName("Plugins.Widgets.ProductRibbon.Fields.Text")]
        public string Text { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.ProductRibbon.Fields.BackgroundColor")]
        public string BackgroundColor { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.ProductRibbon.Fields.TextColor")]
        public string TextColor { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.ProductRibbon.Fields.IsActive")]
        public bool IsActive { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.ProductRibbon.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public ProductRibbonProductSearchModel ProductSearchModel { get; set; }
    }
}
