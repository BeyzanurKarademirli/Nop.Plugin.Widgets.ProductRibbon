using System.Collections.Generic;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Plugin.Widgets.ProductRibbon.Domain;

namespace Nop.Plugin.Widgets.ProductRibbon.Services
{
    public interface IProductRibbonService
    {
        #region ProductRibbon CRUD

        Task<IPagedList<Domain.ProductRibbon>> GetPagedRibbonsAsync(int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);
        Task<IList<Domain.ProductRibbon>> GetAllRibbonsAsync(bool showHidden = false);
        Task<Domain.ProductRibbon> GetRibbonByIdAsync(int ribbonId);
        Task InsertRibbonAsync(Domain.ProductRibbon ribbon);
        Task UpdateRibbonAsync(Domain.ProductRibbon ribbon);
        Task DeleteRibbonAsync(Domain.ProductRibbon ribbon);

        #endregion

        #region Mappings

        Task<IList<ProductRibbonMapping>> GetMappingsByRibbonIdAsync(int ribbonId);
        Task<IList<ProductRibbonMapping>> GetMappingsByProductIdAsync(int productId);
        Task SaveProductMappingsAsync(int ribbonId, IList<int> productIds);

        #endregion

        #region Public Cached Methods

        /// <summary>
        /// Belirli bir ürün ID'sine atanmış aktif etiketleri Cache üzerinden hızlıca getirir.
        /// </summary>
        Task<IList<Domain.ProductRibbon>> GetRibbonsByProductIdAsync(int productId);

        #endregion
    }
}
