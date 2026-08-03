using Nop.Core.Caching;

namespace Nop.Plugin.Widgets.ProductRibbon.Services
{
    public static class ProductRibbonDefaults
    {
        /// <summary>
        /// System Name
        /// </summary>
        public static string SystemName => "Widgets.ProductRibbon";

        /// <summary>
        /// Cache prefix
        /// </summary>
        public static string PrefixCacheKey => "Nop.Plugin.Widgets.ProductRibbon";

        /// <summary>
        /// Cache key for ribbons associated with a product ID
        /// {0} : product ID
        /// </summary>
        public static CacheKey ProductRibbonsCacheKey => new CacheKey("Nop.Plugin.Widgets.ProductRibbon.Product-{0}", PrefixCacheKey);

        /// <summary>
        /// Cache key for all active ribbons
        /// </summary>
        public static CacheKey AllActiveRibbonsCacheKey => new CacheKey("Nop.Plugin.Widgets.ProductRibbon.AllActive", PrefixCacheKey);
    }
}
