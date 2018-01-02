namespace UmbracoUdiToIdCache
{
    using System.Collections.Concurrent;
    using System.Data.SqlClient;
    using Umbraco.Core;
    using Umbraco.Core.Models;

    public static class UdiToIdCache
    {
        private static readonly ConcurrentDictionary<Udi, int> Mapping = new ConcurrentDictionary<Udi, int>();

        /// <summary>
        /// Builds the mapping cache via a database query.  To be called on application start-up.
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        public static void BuildCache(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                const string Sql = "SELECT nodeId, uniqueId FROM cmsDocument d INNER JOIN umbracoNode n ON n.id = d.nodeId";
                var command = new SqlCommand(Sql, connection);
                connection.Open();

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Mapping.TryAdd(ParseUdiFromGuidField(reader), reader.GetInt32(0));
                }

                reader.Close();
            }
        }

        private static Udi ParseUdiFromGuidField(SqlDataReader reader)
        {
            return Udi.Parse($"umb://document/{reader.GetGuid(1).ToString().Replace("-", string.Empty)}");
        }

        /// <summary>
        /// Adds an item to the mapping cache if it's not already there.  To be called on node publish.
        /// </summary>
        /// <param name="content">Content item</param>
        public static void AddToCache(IContent content)
        {
            var key = content.GetUdi();
            if (Mapping.ContainsKey(key))
            {
                Mapping.TryAdd(key, content.Id);
            }
        }

        /// <summary>
        /// Gets the Id for a content node given a Udi from the mapping cache
        /// </summary>
        /// <param name="udi">Udi of content node</param>
        /// <param name="value">Id of content node</param>
        /// <returns>True if id found</returns>
        public static bool TryGetId(Udi udi, out int value)
        {
            return Mapping.TryGetValue(udi, out value);
        }
    }
}