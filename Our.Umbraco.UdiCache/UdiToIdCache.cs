using System;
using System.Collections.Concurrent;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;

namespace Our.Umbraco.UdiCache
{
    internal static class GuidToIdCache
    {
        private static readonly ConcurrentDictionary<Guid, int> Forward = new ConcurrentDictionary<Guid, int>();
        private static readonly ConcurrentDictionary<int, Guid> Reverse = new ConcurrentDictionary<int, Guid>();

        private class TempDto
        {
            [Column("id")]
            public int NodeId { get; set; }

            [Column("uniqueID")]
            public Guid UniqueId { get; set; }
        }

        public static void BuildCache(DatabaseContext databaseContext)
        {
            using (var db = databaseContext.Database)
            {
                var rows = db.Query<TempDto>("SELECT uniqueId, id FROM umbracoNode WHERE nodeObjectType = @0 AND trashed = 0;", Constants.ObjectTypes.Document);
                foreach (var row in rows)
                {
                    TryAdd(row.UniqueId, row.NodeId);
                }
            }
        }

        public static void ClearAll()
        {
            Forward.Clear();
            Reverse.Clear();
        }

        public static void TryAdd(IContent content)
        {
            TryAdd(content.Key, content.Id);
        }

        public static void TryAdd(Guid guid, int id)
        {
            Forward.TryAdd(guid, id);
            Reverse.TryAdd(id, guid);
        }

        public static bool TryGetId(Guid key, out int id)
        {
            return Forward.TryGetValue(key, out id);
        }

        public static bool TryGetGuid(int id, out Guid key)
        {
            return Reverse.TryGetValue(id, out key);
        }

        public static void TryRemove(IContent content)
        {
            if (TryRemove(content.Id) == false)
            {
                TryRemove(content.Key);
            }
        }

        public static bool TryRemove(Guid guid)
        {
            return Forward.TryRemove(guid, out int id)
                ? Reverse.TryRemove(id, out guid)
                : false;
        }

        public static bool TryRemove(int id)
        {
            return Reverse.TryRemove(id, out Guid guid)
                ? Forward.TryRemove(guid, out id)
                : false;
        }
    }
}