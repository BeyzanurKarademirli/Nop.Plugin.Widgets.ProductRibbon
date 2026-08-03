using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Widgets.ProductRibbon.Services;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.ProductRibbon.Components
{
    public class ProductRibbonViewComponent : NopViewComponent
    {
        private readonly IProductRibbonService _productRibbonService;

        public ProductRibbonViewComponent(IProductRibbonService productRibbonService)
        {
            _productRibbonService = productRibbonService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
            int productId = 0;

            if (additionalData != null)
            {
                if (additionalData is int id)
                {
                    productId = id;
                }
                else
                {
                    var idProperty = additionalData.GetType().GetProperty("Id");
                    if (idProperty != null)
                    {
                        var value = idProperty.GetValue(additionalData);
                        if (value is int pId)
                        {
                            productId = pId;
                        }
                    }
                }
            }

            if (productId == 0)
                return Content(string.Empty);

            // Cache destekli servisimizden ürün etiketlerini getiriyoruz
            var ribbons = await _productRibbonService.GetRibbonsByProductIdAsync(productId);
            if (ribbons == null || ribbons.Count == 0)
                return Content(string.Empty);

            ViewBag.WidgetZone = widgetZone;
            return View("~/Plugins/Widgets.ProductRibbon/Views/Public/Default.cshtml", ribbons);
        }
    }
}
