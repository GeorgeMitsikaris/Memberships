using Memberships.Comparers;
using Memberships.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Memberships.Extensions
{
    public static class ThumbnailsExtensions
    {
        public static async Task<List<int>> GetSubscriptionIdsAsync(string userId = null, ApplicationDbContext db = null)
        {
            try
            {
                if (userId == null) return new List<int>();
                if (db == null) db = ApplicationDbContext.Create();

                return await (from us in db.UserSubscriptions
                              where us.UserId.Equals(userId)
                              select us.SubscriptionId).ToListAsync();
            }
            catch
            {

            }
            return new List<int>();
        }

        public static async Task<IEnumerable<ThumbnailModel>> GetProuductThumbnailsAsync(this IEnumerable<ThumbnailModel> thumbnails, string userId = null, ApplicationDbContext db = null)
        {
            try
            {
                if (userId == null) return new List<ThumbnailModel>();
                if (db == null) db = ApplicationDbContext.Create();

                var subscriptionsIds = await GetSubscriptionIdsAsync(userId, db);

                thumbnails = await (from sp in db.SubscriptionProducts
                                    join p in db.Products
                                    on sp.ProductId equals p.Id
                                    join plt in db.productLinkTexts
                                    on p.ProductLinkTextId equals plt.Id
                                    join pt in db.productTypes
                                    on p.ProductTypeid equals pt.Id
                                    where subscriptionsIds.Contains(sp.SubscriptionId)
                                    select new ThumbnailModel
                                    {
                                        ProductId = p.Id,
                                        SubscriptionId = sp.SubscriptionId,
                                        Title = p.Title,
                                        Description = p.Description,
                                        Link = "/ProductContent/Index/" + p.Id,
                                        ContentTag = pt.Title,
                                        ImageUrl = p.ImageUrl,
                                        TagText = plt.Title
                                    }
                                   ).ToListAsync();

            }
            catch { }
            return thumbnails.Distinct(new ThumbnailEqualityComparer()).OrderBy(t => t.Title);
        }
    }
}