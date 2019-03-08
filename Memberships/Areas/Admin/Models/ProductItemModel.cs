using Memberships.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Memberships.Areas.Admin.Models
{
    public class ProductItemModel
    {
        [DisplayName("Products")]
        public int ProductId { get; set; }

        [DisplayName("Items")]
        public int ItemId { get; set; }

        [DisplayName("Product Title")]
        public string ProductTitle { get; set; }

        [DisplayName("Item Title")]
        public string Itemtitle { get; set; }

        public ICollection<Product> Products { get; set; }

        public ICollection<Item> Items { get; set; }
    }
}