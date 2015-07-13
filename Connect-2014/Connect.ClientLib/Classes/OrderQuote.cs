using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect.ClientLib
{

    public class OrderQuote
    {
        public string __type { get; set; }
        public string OrderId { get; set; }
        public Financialdetail FinancialDetail { get; set; }
        public Lineitemdetail[] LineItemDetails { get; set; }
        public Recipientdetail[] RecipientDetails { get; set; }
        public Offeringdetail1[] OfferingDetails { get; set; }
        public int TotalProductionHoursRequired { get; set; }
        public Discountdetail[] DiscountDetails { get; set; }
        public DateTime CutoffDate { get; set; }
        public DateTime EarliestPossibleShipDate { get; set; }
        public DateTime EarliestExpectedDeliveryDate { get; set; }
        public int TotalEstimatedPackageCount { get; set; }
        public string OrderFriendlyId { get; set; }
        public DateTime Submitted { get; set; }
        public int Status { get; set; }
        public Recipientshippedpackage[] RecipientShippedPackages { get; set; }
    }

}
