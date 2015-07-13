using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect.ClientLib
{

    public class OrderReceipt
    {
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

    public class Financialdetail
    {
        public float NonDeliveryAdjustmentSubtotal { get; set; }
        public float NonDeliveryDiscountAdjustmentSubtotal { get; set; }
        public float NonDeliveryDiscountSubtotal { get; set; }
        public float NonDeliverySubtotal { get; set; }
        public float NonDeliveryOneTimeChargeAdjustmentSubtotal { get; set; }
        public float NonDeliveryOneTimeChargeDiscountAdjustmentSubtotal { get; set; }
        public float NonDeliveryOneTimeChargeDiscountSubtotal { get; set; }
        public float NonDeliveryOneTimeChargeSubtotal { get; set; }
        public float DeliveryAdjustmentSubtotal { get; set; }
        public float DeliveryDiscountAdjustmentSubtotal { get; set; }
        public float DeliveryDiscountSubtotal { get; set; }
        public float DeliverySubtotal { get; set; }
        public float TaxAdjustmentSubtotal { get; set; }
        public float TaxSubtotal { get; set; }
        public float AdjustmentSubtotal { get; set; }
        public float DiscountSubtotal { get; set; }
        public float Subtotal { get; set; }
        public float Total { get; set; }
        public float HandlingSubtotal { get; set; }
        public float HandlingDiscountSubtotal { get; set; }
        public float HandlingOneTimeChargeSubtotal { get; set; }
        public float HandlingOneTimeChargeDiscountSubtotal { get; set; }
        public float HandlingAdjustmentSubtotal { get; set; }
        public float HandlingDiscountAdjustmentSubtotal { get; set; }
        public float HandlingOneTimeChargeAdjustmentSubtotal { get; set; }
        public float HandlingOneTimeChargeDiscountAdjustmentSubtotal { get; set; }
    }

    public class Lineitemdetail
    {
        public string LineItemId { get; set; }
        public string LineItemFriendlyId { get; set; }
        public Offeringdetail[] OfferingDetails { get; set; }
        public float Price { get; set; }
        public float OneTimeCharge { get; set; }
        public float Discount { get; set; }
        public float Adjustment { get; set; }
        public float OneTimeChargeDiscount { get; set; }
        public float OneTimeChargeAdjustment { get; set; }
        public float DiscountAdjustment { get; set; }
        public float OneTimeChargeDiscountAdjustment { get; set; }
        public string ReferenceData { get; set; }
        public int ProductionHoursRequired { get; set; }
        public int Quantity { get; set; }
    }

    public class Offeringdetail
    {
        public int Group { get; set; }
        public Detail[] Details { get; set; }
    }

    public class Detail
    {
        public string OfferingId { get; set; }
        public string OfferingName { get; set; }
        public string OfferingTypeId { get; set; }
        public string OfferingTypeName { get; set; }
        public int Quantity { get; set; }
        public float ListPrice { get; set; }
        public float Price { get; set; }
        public float ListOneTimeCharge { get; set; }
        public float OneTimeCharge { get; set; }
        public int ProductionHoursRequired { get; set; }
        public float DiscountedAmount { get; set; }
        public float DiscountedOneTimeChargeAmount { get; set; }
        public DateTime OfferingRetirementDate { get; set; }
        public DateTime OfferingExpirationDate { get; set; }
        public string Code { get; set; }
    }

    public class Recipientdetail
    {
        public string RecipientId { get; set; }
        public float Tax { get; set; }
        public DateTime ExpectedShipDate { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public float DeliveryCharge { get; set; }
        public int EstimatedPackageCount { get; set; }
        public string ReferenceData { get; set; }
        public float DeliveryDiscount { get; set; }
        public float DeliveryTax { get; set; }
    }

    public class Offeringdetail1
    {
        public string OfferingId { get; set; }
        public float Price { get; set; }
        public float OneTimeCharge { get; set; }
        public float Discount { get; set; }
        public float OneTimeChargeDiscount { get; set; }
        public int ProductionHoursRequired { get; set; }
        public int PricingLevel { get; set; }
        public int Quantity { get; set; }
        public string OfferingTypeId { get; set; }
    }

    public class Discountdetail
    {
        public string DiscountCode { get; set; }
        public float NonDeliveryDiscountSubtotal { get; set; }
        public float NonDeliveryOneTimeChargeDiscountSubtotal { get; set; }
        public float DeliveryDiscountSubtotal { get; set; }
        public float HandlingDiscountSubtotal { get; set; }
        public float HandlingOneTimeChargeDiscountSubtotal { get; set; }
    }

    public class Recipientshippedpackage
    {
        public string RecipientId { get; set; }
        public Package[] Packages { get; set; }
    }

    public class Package
    {
        public string TrackingNumber { get; set; }
        public string TrackingUrl { get; set; }
        public ReceiptContent[] Contents { get; set; }
    }

    public class ReceiptContent
    {
        public string FriendlyId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }

}
