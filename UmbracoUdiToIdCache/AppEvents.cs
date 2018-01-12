using Umbraco.Core;
using Umbraco.Core.Cache;
using Umbraco.Core.Models;
using Umbraco.Core.Sync;
using Umbraco.Web.Cache;

namespace Our.Umbraco.UdiCache
{
    public class AppEvents : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            // Build the mapping from Udi to Id on start-up.
            GuidToIdCache.BuildCache(applicationContext.DatabaseContext);

            // On content cache refresh, update the mappings.
            PageCacheRefresher.CacheUpdated += PageCacheRefresher_CacheUpdated;
            UnpublishedPageCacheRefresher.CacheUpdated += UnpublishedPageCacheRefresher_CacheUpdated;
        }

        private void PageCacheRefresher_CacheUpdated(PageCacheRefresher sender, CacheRefresherEventArgs e)
        {
            if (e.MessageType == MessageType.RefreshByInstance && e.MessageObject is IContent instance)
            {
                GuidToIdCache.TryAdd(instance);
            }
        }

        private void UnpublishedPageCacheRefresher_CacheUpdated(UnpublishedPageCacheRefresher sender, CacheRefresherEventArgs e)
        {
            if (e.MessageType == MessageType.RemoveByInstance && e.MessageObject is IContent instance)
            {
                GuidToIdCache.TryRemove(instance);
            }
        }
    }
}