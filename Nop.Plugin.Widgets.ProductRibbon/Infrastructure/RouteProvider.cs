using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Widgets.ProductRibbon.Infrastructure
{
    /// <summary>
    /// Eklenti rotalarını nopCommerce Endpoint Routing sistemine kaydeder.
    /// </summary>
    public class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
        {
            // Admin routes
            endpointRouteBuilder.MapControllerRoute(
                name: "Plugin.Widgets.ProductRibbon.Configure.Admin",
                pattern: "Admin/WidgetsProductRibbon/Configure",
                defaults: new { controller = "WidgetsProductRibbon", action = "Configure", area = AreaNames.Admin });

            endpointRouteBuilder.MapControllerRoute(
                name: "Plugin.Widgets.ProductRibbon.Create.Admin",
                pattern: "Admin/WidgetsProductRibbon/Create",
                defaults: new { controller = "WidgetsProductRibbon", action = "Create", area = AreaNames.Admin });

            endpointRouteBuilder.MapControllerRoute(
                name: "Plugin.Widgets.ProductRibbon.Edit.Admin",
                pattern: "Admin/WidgetsProductRibbon/Edit/{id?}",
                defaults: new { controller = "WidgetsProductRibbon", action = "Edit", area = AreaNames.Admin });

            endpointRouteBuilder.MapControllerRoute(
                name: "Plugin.Widgets.ProductRibbon.Delete.Admin",
                pattern: "Admin/WidgetsProductRibbon/Delete/{id?}",
                defaults: new { controller = "WidgetsProductRibbon", action = "Delete", area = AreaNames.Admin });

            endpointRouteBuilder.MapControllerRoute(
                name: "Plugin.Widgets.ProductRibbon.ProductList.Admin",
                pattern: "Admin/WidgetsProductRibbon/ProductList",
                defaults: new { controller = "WidgetsProductRibbon", action = "ProductList", area = AreaNames.Admin });

            endpointRouteBuilder.MapControllerRoute(
                name: "Plugin.Widgets.ProductRibbon.ProductDelete.Admin",
                pattern: "Admin/WidgetsProductRibbon/ProductDelete",
                defaults: new { controller = "WidgetsProductRibbon", action = "ProductDelete", area = AreaNames.Admin });

            endpointRouteBuilder.MapControllerRoute(
                name: "Plugin.Widgets.ProductRibbon.ProductAddPopup.Admin",
                pattern: "Admin/WidgetsProductRibbon/ProductAddPopup",
                defaults: new { controller = "WidgetsProductRibbon", action = "ProductAddPopup", area = AreaNames.Admin });

            endpointRouteBuilder.MapControllerRoute(
                name: "Plugin.Widgets.ProductRibbon.ProductAddPopupList.Admin",
                pattern: "Admin/WidgetsProductRibbon/ProductAddPopupList",
                defaults: new { controller = "WidgetsProductRibbon", action = "ProductAddPopupList", area = AreaNames.Admin });

            // Plugins routes
            endpointRouteBuilder.MapControllerRoute(
                name: "Plugin.Widgets.ProductRibbon.Configure",
                pattern: "Plugins/WidgetsProductRibbon/Configure",
                defaults: new { controller = "WidgetsProductRibbon", action = "Configure", area = AreaNames.Admin });

            endpointRouteBuilder.MapControllerRoute(
                name: "Plugin.Widgets.ProductRibbon.Create",
                pattern: "Plugins/WidgetsProductRibbon/Create",
                defaults: new { controller = "WidgetsProductRibbon", action = "Create", area = AreaNames.Admin });

            endpointRouteBuilder.MapControllerRoute(
                name: "Plugin.Widgets.ProductRibbon.Edit",
                pattern: "Plugins/WidgetsProductRibbon/Edit/{id?}",
                defaults: new { controller = "WidgetsProductRibbon", action = "Edit", area = AreaNames.Admin });

            endpointRouteBuilder.MapControllerRoute(
                name: "Plugin.Widgets.ProductRibbon.Delete",
                pattern: "Plugins/WidgetsProductRibbon/Delete/{id?}",
                defaults: new { controller = "WidgetsProductRibbon", action = "Delete", area = AreaNames.Admin });

            endpointRouteBuilder.MapControllerRoute(
                name: "Plugin.Widgets.ProductRibbon.ProductList",
                pattern: "Plugins/WidgetsProductRibbon/ProductList",
                defaults: new { controller = "WidgetsProductRibbon", action = "ProductList", area = AreaNames.Admin });

            endpointRouteBuilder.MapControllerRoute(
                name: "Plugin.Widgets.ProductRibbon.ProductDelete",
                pattern: "Plugins/WidgetsProductRibbon/ProductDelete",
                defaults: new { controller = "WidgetsProductRibbon", action = "ProductDelete", area = AreaNames.Admin });

            endpointRouteBuilder.MapControllerRoute(
                name: "Plugin.Widgets.ProductRibbon.ProductAddPopup",
                pattern: "Plugins/WidgetsProductRibbon/ProductAddPopup",
                defaults: new { controller = "WidgetsProductRibbon", action = "ProductAddPopup", area = AreaNames.Admin });

            endpointRouteBuilder.MapControllerRoute(
                name: "Plugin.Widgets.ProductRibbon.ProductAddPopupList",
                pattern: "Plugins/WidgetsProductRibbon/ProductAddPopupList",
                defaults: new { controller = "WidgetsProductRibbon", action = "ProductAddPopupList", area = AreaNames.Admin });
        }

        public int Priority => 1000;
    }
}
