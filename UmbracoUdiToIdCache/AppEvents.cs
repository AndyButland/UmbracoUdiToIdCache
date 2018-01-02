namespace UmbracoUdiToIdCache
{
    using Umbraco.Core;
    using Umbraco.Core.Events;
    using Umbraco.Core.Models;
    using Umbraco.Core.Publishing;
    using Umbraco.Core.Services;

    public class AppEvents : IApplicationEventHandler
    {
        public void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
        }

        public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
        }

        public void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            // Build the mapping from Udi to Id on start-up.
            UdiToIdCache.BuildCache(ApplicationContext.Current.DatabaseContext.ConnectionString);

            // On content publish, add the mapping for the created node if not already there.
            ContentService.Published += ContentServicePublished;
        }

        private static void ContentServicePublished(IPublishingStrategy sender, PublishEventArgs<IContent> args)
        {
            foreach (var node in args.PublishedEntities)
            {
                UdiToIdCache.AddToCache(node);
            }
        }
    }
}