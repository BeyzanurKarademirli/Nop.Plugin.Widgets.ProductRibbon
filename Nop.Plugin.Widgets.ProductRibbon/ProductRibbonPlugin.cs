using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Nop.Core;
using Nop.Core.Domain.Cms;
using Nop.Plugin.Widgets.ProductRibbon.Components;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Menu;

namespace Nop.Plugin.Widgets.ProductRibbon
{
    /// <summary>
    /// Ürün Etiketi Rozetleri (Product Ribbon Widget) Ana Eklenti Sınıfı.
    /// </summary>
    public class ProductRibbonPlugin : BasePlugin, IWidgetPlugin, IAdminMenuPlugin
    {
        #region Fields

        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly ILocalizationService _localizationService;
        private readonly ISettingService _settingService;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly WidgetSettings _widgetSettings;

        #endregion

        #region Ctor

        public ProductRibbonPlugin(
            IActionContextAccessor actionContextAccessor,
            ILocalizationService localizationService,
            ISettingService settingService,
            IUrlHelperFactory urlHelperFactory,
            WidgetSettings widgetSettings)
        {
            _actionContextAccessor = actionContextAccessor;
            _localizationService = localizationService;
            _settingService = settingService;
            _urlHelperFactory = urlHelperFactory;
            _widgetSettings = widgetSettings;
        }

        #endregion

        #region Properties

        public bool HideInWidgetList => false;

        #endregion

        #region Methods

        /// <summary>
        /// Widget'ın tetikleneceği widget alanlarını (Widget Zones) döner.
        /// Ürün kutularının üzerinde görünmesi için ProductBoxAddinfoBefore ve ProductBoxAddinfoAfter alanları eklenmiştir.
        /// </summary>
        public Task<IList<string>> GetWidgetZonesAsync()
        {
            return Task.FromResult<IList<string>>(new List<string>
            {
                PublicWidgetZones.ProductBoxAddinfoBefore,
                PublicWidgetZones.ProductDetailsOverviewTop
            });
        }

        public Type GetWidgetViewComponent(string widgetZone)
        {
            return typeof(ProductRibbonViewComponent);
        }

        public string GetWidgetViewComponentName(string widgetZone)
        {
            return "ProductRibbon";
        }

        public override string GetConfigurationPageUrl()
        {
            return _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext).RouteUrl("Plugin.Widgets.ProductRibbon.Configure");
        }

        public async Task ManageSiteMapAsync(SiteMapNode rootNode)
        {
            var title = await _localizationService.GetResourceAsync("Plugins.Widgets.ProductRibbon.AdminMenuTitle") ?? "Ürün Etiketleri (Ribbons)";

            var pluginNode = new SiteMapNode
            {
                SystemName = "Widgets.ProductRibbon",
                Title = title,
                ControllerName = "WidgetsProductRibbon",
                ActionName = "Configure",
                IconClass = "fas fa-tags",
                Visible = true
            };

            var configNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "Configuration");
            if (configNode != null)
            {
                configNode.ChildNodes.Add(pluginNode);
            }
            else
            {
                rootNode.ChildNodes.Add(pluginNode);
            }
        }

        public override async Task InstallAsync()
        {
            await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
            {
                ["Plugins.Widgets.ProductRibbon.AdminMenuTitle"] = "Ürün Etiketleri (Ribbons)",
                ["Plugins.Widgets.ProductRibbon.PageTitle"] = "Ürün Etiketleri Yönetimi",
                ["Plugins.Widgets.ProductRibbon.Fields.Text"] = "Etiket Metni",
                ["Plugins.Widgets.ProductRibbon.Fields.Text.Hint"] = "Örn: 'Yeni', 'Çok Satan', '%20 İndirim', 'Tükendi'",
                ["Plugins.Widgets.ProductRibbon.Fields.BackgroundColor"] = "Arka Plan Rengi",
                ["Plugins.Widgets.ProductRibbon.Fields.BackgroundColor.Hint"] = "Etiketin arka plan HEX renk kodu.",
                ["Plugins.Widgets.ProductRibbon.Fields.TextColor"] = "Yazı Rengi",
                ["Plugins.Widgets.ProductRibbon.Fields.TextColor.Hint"] = "Etiketin üzerindeki yazının HEX renk kodu.",
                ["Plugins.Widgets.ProductRibbon.Fields.DisplayOrder"] = "Gösterim Sırası",
                ["Plugins.Widgets.ProductRibbon.Fields.DisplayOrder.Hint"] = "Etiketin öncelik sırası.",
                ["Plugins.Widgets.ProductRibbon.Fields.IsActive"] = "Aktif mi?",
                ["Plugins.Widgets.ProductRibbon.Fields.IsActive.Hint"] = "Etiketin yayında olup olmayacağını belirler.",
                ["Plugins.Widgets.ProductRibbon.Fields.Products"] = "Eşleştirilecek Ürünler",
                ["Plugins.Widgets.ProductRibbon.Fields.Products.Hint"] = "Bu etiketin gösterileceği ürünleri seçiniz.",
                ["Plugins.Widgets.ProductRibbon.TextRequired"] = "Etiket metni boş bırakılamaz.",
                ["Plugins.Widgets.ProductRibbon.AddedSuccess"] = "Ürün etiketi başarıyla eklendi.",
                ["Plugins.Widgets.ProductRibbon.UpdatedSuccess"] = "Ürün etiketi başarıyla güncellendi.",
                ["Plugins.Widgets.ProductRibbon.DeletedSuccess"] = "Ürün etiketi başarıyla silindi."
            });

            if (!_widgetSettings.ActiveWidgetSystemNames.Contains(PluginDescriptor.SystemName))
            {
                _widgetSettings.ActiveWidgetSystemNames.Add(PluginDescriptor.SystemName);
                await _settingService.SaveSettingAsync(_widgetSettings);
            }

            await base.InstallAsync();
        }

        public override async Task UninstallAsync()
        {
            if (_widgetSettings.ActiveWidgetSystemNames.Contains(PluginDescriptor.SystemName))
            {
                _widgetSettings.ActiveWidgetSystemNames.Remove(PluginDescriptor.SystemName);
                await _settingService.SaveSettingAsync(_widgetSettings);
            }

            await _localizationService.DeleteLocaleResourcesAsync("Plugins.Widgets.ProductRibbon");
            await base.UninstallAsync();
        }

        #endregion
    }
}
