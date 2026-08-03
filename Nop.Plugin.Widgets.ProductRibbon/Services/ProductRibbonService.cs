using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Data;
using Nop.Plugin.Widgets.ProductRibbon.Domain;

namespace Nop.Plugin.Widgets.ProductRibbon.Services
{
    public class ProductRibbonService : IProductRibbonService
    {
        #region Fields

        private readonly IRepository<Domain.ProductRibbon> _ribbonRepository;
        private readonly IRepository<ProductRibbonMapping> _mappingRepository;
        private readonly IStaticCacheManager _staticCacheManager;

        #endregion

        #region Ctor

        public ProductRibbonService(
            IRepository<Domain.ProductRibbon> ribbonRepository,
            IRepository<ProductRibbonMapping> mappingRepository,
            IStaticCacheManager staticCacheManager)
        {
            _ribbonRepository = ribbonRepository;
            _mappingRepository = mappingRepository;
            _staticCacheManager = staticCacheManager;
        }

        #endregion

        #region ProductRibbon CRUD

        public async Task<IPagedList<Domain.ProductRibbon>> GetPagedRibbonsAsync(int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _ribbonRepository.Table;
            if (!showHidden)
            {
                query = query.Where(r => r.IsActive);
            }

            query = query.OrderBy(r => r.DisplayOrder).ThenBy(r => r.Id);

            return await query.ToPagedListAsync(pageIndex, pageSize);
        }

        public async Task<IList<Domain.ProductRibbon>> GetAllRibbonsAsync(bool showHidden = false)
        {
            var query = _ribbonRepository.Table;
            if (!showHidden)
            {
                query = query.Where(r => r.IsActive);
            }

            query = query.OrderBy(r => r.DisplayOrder).ThenBy(r => r.Id);

            return await query.ToListAsync();
        }

        public async Task<Domain.ProductRibbon> GetRibbonByIdAsync(int ribbonId)
        {
            if (ribbonId == 0)
                return null;

            return await _ribbonRepository.GetByIdAsync(ribbonId);
        }

        public async Task InsertRibbonAsync(Domain.ProductRibbon ribbon)
        {
            if (ribbon == null)
                throw new ArgumentNullException(nameof(ribbon));

            await _ribbonRepository.InsertAsync(ribbon);
            await _staticCacheManager.RemoveByPrefixAsync(ProductRibbonDefaults.PrefixCacheKey);
        }

        public async Task UpdateRibbonAsync(Domain.ProductRibbon ribbon)
        {
            if (ribbon == null)
                throw new ArgumentNullException(nameof(ribbon));

            await _ribbonRepository.UpdateAsync(ribbon);
            await _staticCacheManager.RemoveByPrefixAsync(ProductRibbonDefaults.PrefixCacheKey);
        }

        public async Task DeleteRibbonAsync(Domain.ProductRibbon ribbon)
        {
            if (ribbon == null)
                throw new ArgumentNullException(nameof(ribbon));

            // Silinen etikete ait eşleştirmeleri temizle
            var mappings = await GetMappingsByRibbonIdAsync(ribbon.Id);
            if (mappings.Any())
            {
                await _mappingRepository.DeleteAsync(mappings);
            }

            await _ribbonRepository.DeleteAsync(ribbon);
            await _staticCacheManager.RemoveByPrefixAsync(ProductRibbonDefaults.PrefixCacheKey);
        }

        #endregion

        #region Mappings

        public async Task<IList<ProductRibbonMapping>> GetMappingsByRibbonIdAsync(int ribbonId)
        {
            if (ribbonId == 0)
                return new List<ProductRibbonMapping>();

            return await _mappingRepository.Table
                .Where(m => m.ProductRibbonId == ribbonId)
                .ToListAsync();
        }

        public async Task<IList<ProductRibbonMapping>> GetMappingsByProductIdAsync(int productId)
        {
            if (productId == 0)
                return new List<ProductRibbonMapping>();

            return await _mappingRepository.Table
                .Where(m => m.ProductId == productId)
                .ToListAsync();
        }

        public async Task SaveProductMappingsAsync(int ribbonId, IList<int> productIds)
        {
            if (ribbonId == 0)
                return;

            productIds ??= new List<int>();

            // Mevcut eşleştirmeleri al
            var existingMappings = await GetMappingsByRibbonIdAsync(ribbonId);
            var existingProductIds = existingMappings.Select(m => m.ProductId).ToList();

            // Silinecekler
            var toDelete = existingMappings.Where(m => !productIds.Contains(m.ProductId)).ToList();
            if (toDelete.Any())
            {
                await _mappingRepository.DeleteAsync(toDelete);
            }

            // Eklenecekler
            var toAddProductIds = productIds.Where(id => !existingProductIds.Contains(id)).Distinct();
            var newMappings = toAddProductIds.Select(pid => new ProductRibbonMapping
            {
                ProductRibbonId = ribbonId,
                ProductId = pid
            }).ToList();

            if (newMappings.Any())
            {
                await _mappingRepository.InsertAsync(newMappings);
            }

            await _staticCacheManager.RemoveByPrefixAsync(ProductRibbonDefaults.PrefixCacheKey);
        }

        #endregion

        #region Public Cached Methods

        public async Task<IList<Domain.ProductRibbon>> GetRibbonsByProductIdAsync(int productId)
        {
            if (productId == 0)
                return new List<Domain.ProductRibbon>();

            // nopCommerce IStaticCacheManager ile performanslı önbellekleme
            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(ProductRibbonDefaults.ProductRibbonsCacheKey, productId);

            return await _staticCacheManager.GetAsync(cacheKey, async () =>
            {
                var query = from r in _ribbonRepository.Table
                            join m in _mappingRepository.Table on r.Id equals m.ProductRibbonId
                            where m.ProductId == productId && r.IsActive
                            orderby r.DisplayOrder, r.Id
                            select r;

                return await query.ToListAsync();
            });
        }

        #endregion
    }
}
