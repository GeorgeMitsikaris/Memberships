using Memberships.Areas.Admin.Models;
using Memberships.Entities;
using Memberships.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Memberships.Areas.Admin.Extensions
{
    public static class ConversionExtensions
    {
        #region Products
        public static async Task<IEnumerable<ProductModel>> Convert(this IEnumerable<Product> products, ApplicationDbContext db)
        {
            if (products.Count().Equals(0)) return new List<ProductModel>();

            var texts = await db.productLinkTexts.ToListAsync();
            var types = await db.productTypes.ToListAsync();

            return from p in products
                   select new ProductModel
                   {
                       Id = p.Id,
                       Title = p.Title,
                       Description = p.Description,
                       ImageUrl = p.ImageUrl,
                       ProductLinkTextId = p.ProductLinkTextId,
                       ProductTypeId = p.ProductTypeid,
                       ProductLinkTexts = texts,
                       ProductTypes = types
                   };
        }

        public static async Task<ProductModel> Convert(this Product product, ApplicationDbContext db)
        {
            var text = await db.productLinkTexts.FirstOrDefaultAsync(p => p.Id == product.ProductLinkTextId);
            var type = await db.productTypes.FirstOrDefaultAsync(p => p.Id == product.ProductTypeid);

            var model = new ProductModel
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                ProductLinkTextId = product.ProductLinkTextId,
                ProductTypeId = product.ProductTypeid,
                ProductLinkTexts = new List<ProductLinkText>(),
                ProductTypes = new List<ProductType>()
            };

            model.ProductLinkTexts.Add(text);
            model.ProductTypes.Add(type);

            return model;
        }
        #endregion

        #region ProductItems
        public static async Task<IEnumerable<ProductItemModel>> Convert(this IQueryable<ProductItem> productItems, ApplicationDbContext db)
        {
            if (productItems.Count() == 0)
                return new List<ProductItemModel>();

            return await (from productItem in productItems
                          select new ProductItemModel
                          {
                              ItemId = productItem.ItemId,
                              ProductId = productItem.ProductId,
                              Itemtitle = db.Items.FirstOrDefault(pi=>pi.Id.Equals(productItem.ItemId)).Title,
                              ProductTitle = db.Products.FirstOrDefault(pi=>pi.Id.Equals(productItem.ProductId)).Title,
                          }).ToListAsync();
        }

        public static async Task<ProductItemModel> Convert(this ProductItem productItem, ApplicationDbContext db, bool addList = true)
        {
            return new ProductItemModel
            {
                ProductId = productItem.ProductId,
                ItemId = productItem.ItemId,
                Products = addList ? await db.Products.ToListAsync() : null,
                Items = addList ? await db.Items.ToListAsync() : null,
                ProductTitle = (await db.Products.FirstOrDefaultAsync(p=>p.Id.Equals(productItem.ProductId))).Title,
                Itemtitle = (await db.Items.FirstOrDefaultAsync(i=>i.Id.Equals(productItem.ItemId))).Title
            };
        }

        public static async Task<bool> CanChange(this ProductItem productItem, ApplicationDbContext db)
        {
            var oldProductItem = await db.ProductItems.CountAsync(pi => pi.ProductId.Equals(productItem.OldProductId) && pi.ItemId.Equals(productItem.OldItemId));

            var newProductItem = await db.ProductItems.CountAsync(pi => pi.ProductId.Equals(productItem.ProductId) && pi.ItemId.Equals(productItem.ItemId));

            return oldProductItem.Equals(1) && newProductItem.Equals(0);           
        }

        public static async Task Change(this ProductItem productItem, ApplicationDbContext db)
        {
            var oldProductItem = await db.ProductItems.FirstOrDefaultAsync(pi => pi.ProductId.Equals(productItem.OldProductId) && pi.ItemId.Equals(productItem.OldItemId));

            var newProductItem = await db.ProductItems.FirstOrDefaultAsync(pi => pi.ProductId.Equals(productItem.ProductId) && pi.ItemId.Equals(productItem.ItemId));

            if (oldProductItem != null && newProductItem == null)
            {
                newProductItem = new ProductItem
                {
                    ItemId = productItem.ItemId,
                    ProductId = productItem.ProductId
                };

                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        db.ProductItems.Remove(oldProductItem);
                        db.ProductItems.Add(newProductItem);
                        await db.SaveChangesAsync();
                        transaction.Complete();
                    }

                    catch { transaction.Dispose(); }
                }
            }
        }
        #endregion

        #region SubscriptionProducts
        public static async Task<IEnumerable<SubscriptionProductModel>> Convert(this IQueryable<SubscriptionProduct> subscriptionProducts, ApplicationDbContext db)
        {
            if (subscriptionProducts.Count() == 0)
                return new List<SubscriptionProductModel>();

            return await (from subscriptionProduct in subscriptionProducts
                          select new SubscriptionProductModel
                          {
                              SubscriptionId = subscriptionProduct.SubscriptionId,
                              ProductId = subscriptionProduct.ProductId,
                              Subscriptiontitle = db.Subscriptions.FirstOrDefault(pi => pi.Id.Equals(subscriptionProduct.SubscriptionId)).Title,
                              ProductTitle = db.Products.FirstOrDefault(pi => pi.Id.Equals(subscriptionProduct.ProductId)).Title,
                          }).ToListAsync();
        }

        public static async Task<SubscriptionProductModel> Convert(this SubscriptionProduct subscriptionProduct, ApplicationDbContext db, bool addList = true)
        {
            return new SubscriptionProductModel
            {
                ProductId = subscriptionProduct.ProductId,
                SubscriptionId = subscriptionProduct.SubscriptionId,
                Products = addList ? await db.Products.ToListAsync() : null,
                Subscriptions = addList ? await db.Subscriptions.ToListAsync() : null,
                ProductTitle = (await db.Products.FirstOrDefaultAsync(p => p.Id.Equals(subscriptionProduct.ProductId))).Title,
                Subscriptiontitle = (await db.Subscriptions.FirstOrDefaultAsync(i => i.Id.Equals(subscriptionProduct.SubscriptionId))).Title
            };
        }

        public static async Task<bool> CanChange(this SubscriptionProduct subscriptionProduct, ApplicationDbContext db)
        {
            var oldsubscriptionProduct = await db.SubscriptionProducts.CountAsync(pi => pi.ProductId.Equals(subscriptionProduct.OldProductId) && pi.SubscriptionId.Equals(subscriptionProduct.OldSubscriptionId));

            var newsubscriptionProduct = await db.SubscriptionProducts.CountAsync(pi => pi.ProductId.Equals(subscriptionProduct.ProductId) && pi.SubscriptionId.Equals(subscriptionProduct.SubscriptionId));

            return oldsubscriptionProduct.Equals(1) && newsubscriptionProduct.Equals(0);
        }

        public static async Task Change(this SubscriptionProduct subscriptionProduct, ApplicationDbContext db)
        {
            var oldSubscriptionProduct = await db.SubscriptionProducts.FirstOrDefaultAsync(pi => pi.ProductId.Equals(subscriptionProduct.OldProductId) && pi.SubscriptionId.Equals(subscriptionProduct.OldSubscriptionId));

            var newSubscriptionProduct = await db.SubscriptionProducts.FirstOrDefaultAsync(pi => pi.ProductId.Equals(subscriptionProduct.ProductId) && pi.SubscriptionId.Equals(subscriptionProduct.SubscriptionId));

            if (oldSubscriptionProduct != null && newSubscriptionProduct == null)
            {
                newSubscriptionProduct = new SubscriptionProduct
                {
                    SubscriptionId = subscriptionProduct.SubscriptionId,
                    ProductId = subscriptionProduct.ProductId
                };

                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        db.SubscriptionProducts.Remove(oldSubscriptionProduct);
                        db.SubscriptionProducts.Add(newSubscriptionProduct);
                        await db.SaveChangesAsync();
                        transaction.Complete();
                    }

                    catch { transaction.Dispose(); }
                }
            }
        }
        #endregion
    }
}