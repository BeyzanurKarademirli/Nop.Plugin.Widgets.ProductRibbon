using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Widgets.ProductRibbon.Domain;
using Nop.Plugin.Widgets.ProductRibbon.Models;
using Nop.Plugin.Widgets.ProductRibbon.Services;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Widgets.ProductRibbon.Controllers
{
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    [AutoValidateAntiforgeryToken]
    public class WidgetsProductRibbonController : BasePluginController
    {
        #region Fields

        private readonly IProductRibbonService _productRibbonService;
        private readonly IProductService _productService;
        private readonly INotificationService _notificationService;
        private readonly ILocalizationService _localizationService;
        private readonly ILogger _logger;

        #endregion

        #region Ctor

        public WidgetsProductRibbonController(
            IProductRibbonService productRibbonService,
            IProductService productService,
            INotificationService notificationService,
            ILocalizationService localizationService,
            ILogger logger)
        {
            _productRibbonService = productRibbonService;
            _productService = productService;
            _notificationService = notificationService;
            _localizationService = localizationService;
            _logger = logger;
        }

        #endregion

        #region Methods

        private async Task PrepareProductRibbonModelAsync(ProductRibbonModel model, Domain.ProductRibbon ribbon = null)
        {
            if (ribbon != null)
            {
                var mappings = await _productRibbonService.GetMappingsByRibbonIdAsync(ribbon.Id);
                model.SelectedProductIds = mappings.Select(m => m.ProductId).ToList();
            }

            // Mevcut ürünleri SelectListItem olarak doldur
            var products = await _productService.SearchProductsAsync(showHidden: true);
            model.AvailableProducts = products.Select(p => new SelectListItem
            {
                Text = $"{p.Name} (ID: {p.Id})",
                Value = p.Id.ToString(),
                Selected = model.SelectedProductIds.Contains(p.Id)
            }).ToList();
        }

        #endregion

        #region Actions

        [HttpGet]
        public async Task<IActionResult> Configure()
        {
            var ribbons = await _productRibbonService.GetAllRibbonsAsync(showHidden: true);

            var modelList = ribbons.Select(r => new ProductRibbonModel
            {
                Id = r.Id,
                Text = r.Text,
                BackgroundColor = r.BackgroundColor,
                TextColor = r.TextColor,
                IsActive = r.IsActive,
                DisplayOrder = r.DisplayOrder
            }).ToList();

            ViewBag.Ribbons = modelList;

            var newModel = new ProductRibbonModel
            {
                BackgroundColor = "#ef4444",
                TextColor = "#ffffff",
                IsActive = true,
                DisplayOrder = 1
            };

            await PrepareProductRibbonModelAsync(newModel);

            return View("~/Plugins/Widgets.ProductRibbon/Views/Configure.cshtml", newModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductRibbonModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Text))
            {
                var reqMsg = await _localizationService.GetResourceAsync("Plugins.Widgets.ProductRibbon.TextRequired") ?? "Etiket metni boş bırakılamaz.";
                _notificationService.ErrorNotification(reqMsg);
                return RedirectToAction("Configure");
            }

            var ribbon = new Domain.ProductRibbon
            {
                Text = model.Text,
                BackgroundColor = string.IsNullOrWhiteSpace(model.BackgroundColor) ? "#ef4444" : model.BackgroundColor,
                TextColor = string.IsNullOrWhiteSpace(model.TextColor) ? "#ffffff" : model.TextColor,
                IsActive = model.IsActive,
                DisplayOrder = model.DisplayOrder
            };

            await _productRibbonService.InsertRibbonAsync(ribbon);

            // Ürün eşleştirmelerini kaydet
            await _productRibbonService.SaveProductMappingsAsync(ribbon.Id, model.SelectedProductIds);

            var successMsg = await _localizationService.GetResourceAsync("Plugins.Widgets.ProductRibbon.AddedSuccess") ?? "Etiket başarıyla eklendi.";
            _notificationService.SuccessNotification(successMsg);

            return RedirectToAction("Configure");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var ribbon = await _productRibbonService.GetRibbonByIdAsync(id);
            if (ribbon == null)
                return RedirectToAction("Configure");

            var model = new ProductRibbonModel
            {
                Id = ribbon.Id,
                Text = ribbon.Text,
                BackgroundColor = ribbon.BackgroundColor,
                TextColor = ribbon.TextColor,
                IsActive = ribbon.IsActive,
                DisplayOrder = ribbon.DisplayOrder
            };

            await PrepareProductRibbonModelAsync(model, ribbon);

            var allRibbons = await _productRibbonService.GetAllRibbonsAsync(showHidden: true);
            ViewBag.Ribbons = allRibbons.Select(r => new ProductRibbonModel
            {
                Id = r.Id,
                Text = r.Text,
                BackgroundColor = r.BackgroundColor,
                TextColor = r.TextColor,
                IsActive = r.IsActive,
                DisplayOrder = r.DisplayOrder
            }).ToList();

            return View("~/Plugins/Widgets.ProductRibbon/Views/Configure.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductRibbonModel model)
        {
            var ribbon = await _productRibbonService.GetRibbonByIdAsync(model.Id);
            if (ribbon == null)
                return RedirectToAction("Configure");

            if (string.IsNullOrWhiteSpace(model.Text))
            {
                var reqMsg = await _localizationService.GetResourceAsync("Plugins.Widgets.ProductRibbon.TextRequired") ?? "Etiket metni boş bırakılamaz.";
                _notificationService.ErrorNotification(reqMsg);
                return RedirectToAction("Configure");
            }

            ribbon.Text = model.Text;
            ribbon.BackgroundColor = string.IsNullOrWhiteSpace(model.BackgroundColor) ? "#ef4444" : model.BackgroundColor;
            ribbon.TextColor = string.IsNullOrWhiteSpace(model.TextColor) ? "#ffffff" : model.TextColor;
            ribbon.IsActive = model.IsActive;
            ribbon.DisplayOrder = model.DisplayOrder;

            await _productRibbonService.UpdateRibbonAsync(ribbon);

            // Ürün eşleştirmelerini güncelle
            await _productRibbonService.SaveProductMappingsAsync(ribbon.Id, model.SelectedProductIds);

            var successMsg = await _localizationService.GetResourceAsync("Plugins.Widgets.ProductRibbon.UpdatedSuccess") ?? "Etiket başarıyla güncellendi.";
            _notificationService.SuccessNotification(successMsg);

            return RedirectToAction("Configure");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var ribbon = await _productRibbonService.GetRibbonByIdAsync(id);
            if (ribbon != null)
            {
                await _productRibbonService.DeleteRibbonAsync(ribbon);
                var successMsg = await _localizationService.GetResourceAsync("Plugins.Widgets.ProductRibbon.DeletedSuccess") ?? "Etiket başarıyla silindi.";
                _notificationService.SuccessNotification(successMsg);
            }

            return RedirectToAction("Configure");
        }

        #endregion
    }
}
