using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Common.Pagination
{
    public static class ListExtension
    {
        public static async Task<DataCollection<T>> GetPagedAsync<T>(
          this IFindFluent<T, T> query,
          int page,
          int take)
        {
            var originalPages = page;

            page--;

            if (page > 0)
                page = page * take;

            var result = new DataCollection<T>
            {
                Total = await query.CountDocumentsAsync(),
                Items = await query
                    .Sort(new SortDefinitionBuilder<T>()
                    .Ascending("Timestamp"))
                    .Skip(page)
                    .Limit(take)
                    .ToListAsync(),               
                Page = originalPages
            };

            if (result.Total > 0)
            {
                result.Pages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(result.Total) / take));
            }

            return result;
        }
    }
}
