using System;
using Our.Umbraco.UdiCache;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace Umbraco.Web
{
    public static class UmbracoHelperExtensions
    {
        /// <summary>
        /// Gets a content item from the cache.
        /// </summary>
        /// <param name="helper">The instance of <see cref="UmbracoHelper"/> to add extension method.</param>
        /// <param name="udi">The <see cref="Udi"/> of the content item.</param>
        /// <returns>The content, or null of the content item is not in the cache.</returns>
        public static IPublishedContent TypedContent(this UmbracoHelper helper, Udi udi, bool usingUdiToIdCache)
        {
            return usingUdiToIdCache && udi is GuidUdi guidUdi && GuidToIdCache.TryGetId(guidUdi.Guid, out int id)
                ? helper.TypedContent(id)
                : helper.TypedContent(udi);
        }

        /// <summary>
        /// Gets a content item from the cache.
        /// </summary>
        /// <param name="helper">The instance of <see cref="UmbracoHelper"/> to add extension method.</param>
        /// <param name="guid">The key of the content item.</param>
        /// <param name="usingUdiToIdCache"></param>
        /// <returns>The content, or null of the content item is not in the cache.</returns>
        public static IPublishedContent TypedContent(this UmbracoHelper helper, Guid guid, bool usingUdiToIdCache)
        {
            return usingUdiToIdCache && GuidToIdCache.TryGetId(guid, out int id)
                ? helper.TypedContent(id)
                : helper.TypedContent(guid);
        }
    }
}