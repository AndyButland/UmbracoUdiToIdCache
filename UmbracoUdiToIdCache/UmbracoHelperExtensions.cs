namespace Umbraco.Web
{
    using Umbraco.Core;
    using Umbraco.Core.Models;
    using Umbraco.Web;
    using UmbracoUdiToIdCache;

    public static class UmbracoHelperExtensions
    {
        /// <summary>
        /// Adds an extension to <see cref="UmbracoHelper"/> to make a call to TypedContent for a Udi
        /// via a look-up to get the numeric Id
        /// </summary>
        /// <param name="helper">Umbraco helper</param>
        /// <param name="udi">Udi of content node</param>
        /// <returns>Instance of <see cref="IPublishedContent"/></returns>
        public static IPublishedContent TypedContentUsingUdiToIdCache(this UmbracoHelper helper, Udi udi)
        {
            return UdiToIdCache.TryGetId(udi, out int id) 
                ? helper.TypedContent(id) 
                : helper.TypedContent(udi);
        }
    }
}