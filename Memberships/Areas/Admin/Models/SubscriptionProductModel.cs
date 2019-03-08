using Memberships.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Memberships.Areas.Admin.Models
{
    public class SubscriptionProductModel
    {
        [DisplayName("Products")]
        public int ProductId { get; set; }

        [DisplayName("Subscriptions")]
        public int SubscriptionId { get; set; }

        [DisplayName("Product Title")]
        public string ProductTitle { get; set; }

        [DisplayName("Subscription Title")]
        public string Subscriptiontitle { get; set; }

        public ICollection<Product> Products { get; set; }

        public ICollection<Subscription> Subscriptions { get; set; }
    }
}