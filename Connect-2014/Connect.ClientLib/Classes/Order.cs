using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect.ClientLib.Template
{

    public class Order
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Recipient[] Recipients { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Paymentmethod PaymentMethod { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Specialinstructioncode[] SpecialInstructionCodes { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Discountcode[] DiscountCodes { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Shipfrominfo ShipFromInfo { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Orderoptions OrderOptions { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Packagingslip PackagingSlip { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ReferenceNumber { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OrderFriendlyId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Product Product { get; set; }

        public void Initialize()
        {
            this.Recipients = null;
            this.PaymentMethod = null;
            this.SpecialInstructionCodes = null;
            this.DiscountCodes = null;
            this.ShipFromInfo = null;
            this.OrderOptions = null;
            this.PackagingSlip = null;
            this.ReferenceNumber = null;
            this.OrderFriendlyId = null;
        }
    }

    public class Paymentmethod
    {
        public int PaymentMethodType { get; set; }
        public Creditcard CreditCard { get; set; }
        public Corporateaccount CorporateAccount { get; set; }
        public Creditlimit CreditLimit { get; set; }
    }

    public class Creditcard
    {
        public string Number { get; set; }
        public string NameOnCard { get; set; }
        public string BillingPostalCode { get; set; }
        public int CreditCardType { get; set; }
        public int ExpirationMonth { get; set; }
        public int ExpirationYear { get; set; }
    }

    public class Corporateaccount
    {
    }

    public class Creditlimit
    {
    }

    public class Shipfrominfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string TelephoneNumber { get; set; }
    }

    public class Orderoptions
    {
        public int AdditionalProcessingHours { get; set; }
        public bool TaxExemptStatusEnabled { get; set; }
        public Recipientnotificationoptions RecipientNotificationOptions { get; set; }
    }

    public class Recipientnotificationoptions
    {
        public bool ShouldNotifyRecipients { get; set; }
        public bool IncludeSenderContactInformation { get; set; }
    }

    public class Packagingslip
    {
        public int SalutationType { get; set; }
        public string Memo { get; set; }
    }

    public class Product
    {
        public string __type { get; set; }
        public string ApplicationId { get; set; }
        public int Quantity { get; set; }
        public string Template { get; set; }
        public string DocumentTemplateId { get; set; }
        public string DocumentTemplateName { get; set; }
        public Content[] Content { get; set; }
    }

    public class Content
    {
        public string Source { get; set; }
        public string Range { get; set; }
    }

    public class Recipient
    {
        public Address Address { get; set; }
        public string ReferenceData { get; set; }
        public int ShipmentNumber { get; set; }
        public int SignatureReleaseType { get; set; }
        public string ShippingMethodId { get; set; }
        public string DistributionListId { get; set; }
        public string AddressId { get; set; }
    }

    public class Address
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string ApartmentOrSuite { get; set; }
        public string CareOf { get; set; }
        public string City { get; set; }
        public string StateOrProvince { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string TelephoneNumber { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public bool IsResidential { get; set; }
        public string Name { get; set; }
    }

    public class Specialinstructioncode
    {
        public string Code { get; set; }
    }

    public class Discountcode
    {
        public string DiscountCode { get; set; }
    }


}
