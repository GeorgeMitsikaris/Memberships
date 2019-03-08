using System.Text;

namespace Memberships.Areas.Admin.Models
{
    public class EditButtonModel
    {
        public int ItemId { get; set; }
        public int ProductId { get; set; }
        public int SubscriptionId { get; set; }
        public string Link
        {
            get
            {
                StringBuilder sb = new StringBuilder("?");
                if (ItemId > 0)
                {
                    sb.Append(string.Format($"itemId={ItemId}&"));
                }
                if (ProductId > 0)
                {
                    sb.Append(string.Format($"productId={ProductId}&"));
                }
                if (SubscriptionId > 0)
                {
                    sb.Append(string.Format($"subscriptionId={SubscriptionId}&"));
                }

                return sb.ToString().Substring(0, sb.Length - 1);
            }
        }
    }
}