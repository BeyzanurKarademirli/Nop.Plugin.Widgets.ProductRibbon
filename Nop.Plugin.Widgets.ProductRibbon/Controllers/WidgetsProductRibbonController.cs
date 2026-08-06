using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Widgets.ProductRibbon.Domain;
using Nop.Plugin.Widgets.ProductRibbon.Models;
using Nop.Plugin.Widgets.ProductRibbon.Services;
using Nop.Services;
using Nop.Services.Catalog;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Stores;
using Nop.Services.Vendors;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Models.Extensions;
using Nop.Web.Framework.Mvc;
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
        private readonly ICategoryService _categoryService;
        private readonly IManufacturerService _manufacturerService;
        private readonly IStoreService _storeService;
        private readonly IVendorService _vendorService;
        private readonly INotificationService _notificationService;
        private readonly ILocalizationService _localizationService;
        private readonly ILogger _logger;

        #endregion

        #region Ctor

        public WidgetsProductRibbonController(
            IProductRibbonService productRibbonService,
            IProductService productService,
            ICategoryService categoryService,
            IManufacturerService manufacturerService,
            IStoreService storeService,
            IVendorService vendorService,
            INotificationService notificationService,
            ILocalizationService localizationService,
            ILogger logger)
        {
            _productRibbonService = productRibbonService;
            _productService = productService;
            _categoryService = categoryService;
            _manufacturerService = manufacturerService;
            _storeService = storeService;
            _vendorService = vendorService;
            _notificationService = notificationService;
            _localizationService = localizationService;
            _logger = logger;
        }

        #endregion

        #region Utilities

        private Task PrepareProductRibbonModelAsync(ProductRibbonModel model, Domain.ProductRibbon ribbon = null)
        {
            if (ribbon != null)
            {
                model.ProductSearchModel.ProductRibbonId = ribbon.Id;
                model.ProductSearchModel.SetGridPageSize();
            }

            return Task.CompletedTask;
        }

        private async Task PrepareAddProductToRibbonSearchModelAsync(AddProductToRibbonSearchModel searchModel)
        {
            if (searchModel == null)
                return;

            // Categories
            var categories = await _categoryService.GetAllCategoriesAsync(showHidden: true);
            searchModel.AvailableCategories.Add(new SelectListItem { Text = await _localizationService.GetResourceAsync("Admin.Common.All"), Value = "0" });
            foreach (var c in categories)
            {
                searchModel.AvailableCategories.Add(new SelectListItem { Text = await _categoryService.GetFormattedBreadCrumbAsync(c), Value = c.Id.ToString() });
            }

            // Manufacturers
            var manufacturers = await _manufacturerService.GetAllManufacturersAsync(showHidden: true);
            searchModel.AvailableManufacturers.Add(new SelectListItem { Text = await _localizationService.GetResourceAsync("Admin.Common.All"), Value = "0" });
            foreach (var m in manufacturers)
            {
                searchModel.AvailableManufacturers.Add(new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            }

            // Stores
            var stores = await _storeService.GetAllStoresAsync();
            searchModel.AvailableStores.Add(new SelectListItem { Text = await _localizationService.GetResourceAsync("Admin.Common.All"), Value = "0" });
            foreach (var s in stores)
            {
                searchModel.AvailableStores.Add(new SelectListItem { Text = s.Name, Value = s.Id.ToString() });
            }

            // Vendors
            var vendors = await _vendorService.GetAllVendorsAsync(showHidden: true);
            searchModel.AvailableVendors.Add(new SelectListItem { Text = await _localizationService.GetResourceAsync("Admin.Common.All"), Value = "0" });
            foreach (var v in vendors)
            {
                searchModel.AvailableVendors.Add(new SelectListItem { Text = v.Name, Value = v.Id.ToString() });
            }

            // Product Types
            var availableProductTypeItems = await ProductType.SimpleProduct.ToSelectListAsync(false);
            foreach (var ptItem in availableProductTypeItems)
            {
                searchModel.AvailableProductTypes.Add(ptItem);
            }
            searchModel.AvailableProductTypes.Insert(0, new SelectListItem { Text = await _localizationService.GetResourceAsync("Admin.Common.All"), Value = "0" });

            searchModel.SetPopupGridPageSize();
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

            var successMsg = await _localizationService.GetResourceAsync("Plugins.Widgets.ProductRibbon.AddedSuccess") ?? "Etiket başarıyla eklendi.";
            _notificationService.SuccessNotification(successMsg);

            return RedirectToAction("Edit", new { id = ribbon.Id });
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

        #region Product Ribbon Mappings & Popup

        [HttpPost]
        public async Task<IActionResult> ProductList(ProductRibbonProductSearchModel searchModel)
        {
            var mappings = await _productRibbonService.GetMappingsByRibbonIdAsync(searchModel.ProductRibbonId);
            var productIds = mappings.Select(m => m.ProductId).ToArray();
            var products = await _productService.GetProductsByIdsAsync(productIds);

            var pageIndex = searchModel.Page > 0 ? searchModel.Page - 1 : 0;
            var pageSize = searchModel.PageSize > 0 ? searchModel.PageSize : 15;
            var pagedProducts = new PagedList<Product>(products.ToList(), pageIndex, pageSize);

            var model = await new ProductRibbonProductListModel().PrepareToGridAsync(searchModel, pagedProducts, () =>
            {
                return pagedProducts.SelectAwait(product => System.Threading.Tasks.ValueTask.FromResult(new ProductRibbonProductModel
                {
                    Id = product.Id,
                    RibbonId = searchModel.ProductRibbonId,
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Published = product.Published
                }));
            });

            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> ProductDelete(int ribbonId, int id)
        {
            var mappings = await _productRibbonService.GetMappingsByRibbonIdAsync(ribbonId);
            var updatedProductIds = mappings.Where(m => m.ProductId != id).Select(m => m.ProductId).ToList();
            await _productRibbonService.SaveProductMappingsAsync(ribbonId, updatedProductIds);

            return new NullJsonResult();
        }

        [HttpGet]
        public async Task<IActionResult> ProductAddPopup(int ribbonId)
        {
            var model = new AddProductToRibbonSearchModel
            {
                RibbonId = ribbonId
            };

            await PrepareAddProductToRibbonSearchModelAsync(model);

            return View("~/Plugins/Widgets.ProductRibbon/Views/ProductAddPopup.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> ProductAddPopupList(AddProductToRibbonSearchModel searchModel)
        {
            var pageIndex = searchModel.Page > 0 ? searchModel.Page - 1 : 0;
            var pageSize = searchModel.PageSize > 0 ? searchModel.PageSize : 15;

            var existingMappings = await _productRibbonService.GetMappingsByRibbonIdAsync(searchModel.RibbonId);
            var existingProductIds = existingMappings.Select(m => m.ProductId).ToList();

            var products = await _productService.SearchProductsAsync(
                showHidden: true,
                categoryIds: searchModel.SearchCategoryId > 0 ? new List<int> { searchModel.SearchCategoryId } : null,
                manufacturerIds: searchModel.SearchManufacturerId > 0 ? new List<int> { searchModel.SearchManufacturerId } : null,
                storeId: searchModel.SearchStoreId,
                vendorId: searchModel.SearchVendorId,
                productType: searchModel.SearchProductTypeId > 0 ? (ProductType?)searchModel.SearchProductTypeId : null,
                keywords: searchModel.SearchProductName,
                pageIndex: pageIndex,
                pageSize: pageSize
            );

            var unmappedProducts = products.Where(p => !existingProductIds.Contains(p.Id)).ToList();
            var pagedProducts = new PagedList<Product>(unmappedProducts, pageIndex, pageSize, Math.Max(0, products.TotalCount - existingProductIds.Count));

            var model = await new AddProductToRibbonListModel().PrepareToGridAsync(searchModel, pagedProducts, () =>
            {
                return pagedProducts.SelectAwait(product => System.Threading.Tasks.ValueTask.FromResult(new ProductRibbonProductModel
                {
                    Id = product.Id,
                    RibbonId = searchModel.RibbonId,
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Published = product.Published
                }));
            });

            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> ProductAddPopup(AddProductToRibbonModel model)
        {
            if (model.RibbonId > 0 && model.SelectedProductIds != null && model.SelectedProductIds.Any())
            {
                var existingMappings = await _productRibbonService.GetMappingsByRibbonIdAsync(model.RibbonId);
                var existingProductIds = existingMappings.Select(m => m.ProductId).ToList();
                var allProductIds = existingProductIds.Concat(model.SelectedProductIds).Distinct().ToList();

                await _productRibbonService.SaveProductMappingsAsync(model.RibbonId, allProductIds);
            }

            ViewBag.RefreshPage = true;

            return View("~/Plugins/Widgets.ProductRibbon/Views/ProductAddPopup.cshtml", new AddProductToRibbonSearchModel());
        }

        #endregion
    }
}
