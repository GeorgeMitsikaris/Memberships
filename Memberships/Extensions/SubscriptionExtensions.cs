using Memberships.Entities;
using Memberships.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Memberships.Extensions
{
    public static class SubscriptionExtensions
    {
        public static async Task<int> GetSubscriptionIdByRegistratinCode(this IDbSet<Subscription> subscriptions, string code)
        {
            try
            {
                if (subscriptions == null || string.IsNullOrWhiteSpace(code))
                {
                    return Int32.MinValue;
                }
                else
                {
                    return await (from s in subscriptions
                                  where s.RegistrationCode == code
                                  select s.Id).FirstOrDefaultAsync();
                }
            }
            catch { return Int32.MinValue; }
        }

        public static async Task Register(this IDbSet<UserSubscription> userSubscriptions, int subscriptionId, string userId)
        {
            try
            {
                if (userSubscriptions == null || subscriptionId == int.MinValue || string.IsNullOrEmpty(userId)) { return; }

                var exist = await (Task.Run(() => userSubscriptions.CountAsync(us => us.SubscriptionId == subscriptionId && us.UserId == userId))) > 0;

                if (!exist)
                {
                    await (Task.Run(() => userSubscriptions.Add(new UserSubscription
                    {
                        UserId = userId,
                        SubscriptionId = subscriptionId,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.MaxValue
                    })));
                }
            }
            catch { }
        }

        public static async Task<bool> RegisterUserSubscriptionCode(string userId, string code)
        {
            try
            {
                var db = ApplicationDbContext.Create();
                var id = await db.Subscriptions.GetSubscriptionIdByRegistratinCode(code);
                if (id <= 0) return false;

                await db.UserSubscriptions.Register(id, userId);

                if (db.ChangeTracker.HasChanges())
                    await db.SaveChangesAsync();
                return true;
            }

            catch { return false; }
        }
    }
}