using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Widgets.ProductRibbon.Infrastructure
{
    /// <summary>
    /// Represents plugin route provider
    /// </summary>
    public class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapControllerRoute(
                name: "Plugin.Widgets.ProductRibbon.Configure",
                pattern: "Admin/WidgetsProductRibbon/Configure",
                defaults: new { controller = "WidgetsProductRibbon", action = "Configure", area = AreaNames.Admin });

            endpointRouteBuilder.MapControllerRoute(
                name: "Plugin.Widgets.ProductRibbon.Create",
                pattern: "Admin/WidgetsProductRibbon/Create",
                defaults: new { controller = "WidgetsProductRibbon", action = "Create", area = AreaNames.Admin });

            endpointRouteBuilder.MapControllerRoute(
                name: "Plugin.Widgets.ProductRibbon.Edit",
                pattern: "Admin/WidgetsProductRibbon/Edit/{id?}",
                defaults: new { controller = "WidgetsProductRibbon", action = "Edit", area = AreaNames.Admin });

            endpointRouteBuilder.MapControllerRoute(
                name: "Plugin.Widgets.ProductRibbon.Delete",
                pattern: "Admin/WidgetsProductRibbon/Delete/{id?}",
                defaults: new { controller = "WidgetsProductRibbon", action = "Delete", area = AreaNames.Admin });
        }

        public int Priority => 0;
    }
}

