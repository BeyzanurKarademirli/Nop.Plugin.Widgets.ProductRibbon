using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.Widgets.ProductRibbon.Services;

namespace Nop.Plugin.Widgets.ProductRibbon.Infrastructure
{
    /// <summary>
    /// Servislerimizi nopCommerce Dependency Injection (DI) konteynerine kaydeder.
    /// </summary>
    public class NopStartup : INopStartup
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Servisimiz IoC konteynerine Scoped olarak kaydediliyor
            services.AddScoped<IProductRibbonService, ProductRibbonService>();
        }

        public void Configure(IApplicationBuilder application)
        {
        }

        public int Order => 3000;
    }
}
