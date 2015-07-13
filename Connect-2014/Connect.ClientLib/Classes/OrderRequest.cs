using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect.ClientLib
{

    public class OrderRequest
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Lineitem> LineItems { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Recipient> Recipients { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> SpecialInstructionCodes { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> DiscountCodes { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Paymentmethod PaymentMethod { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Shipfrominfo ShipFromInfo { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Packagingslip PackagingSlip { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ReferenceNumber { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Options Options { get; set; }
    }

    public class Paymentmethod
    {

        [JsonProperty("$type", NullValueHandling = NullValueHandling.Ignore)]
        public string type { get; set; }
        [JsonProperty("__type", NullValueHandling = NullValueHandling.Ignore)]
        public string __type { get; set; } 
    }

    public class Shipfrominfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string TelephoneNumber { get; set; }
    }

    public class Packagingslip
    {
        public int SalutationType { get; set; }
        public string Memo { get; set; }
    }

    public class Options
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

    public class Lineitem
    {
        public string Name { get; set; }
        public Storeitemreference2 StoreItemReference { get; set; }
        public string Description { get; set; }
        public string ReferenceData { get; set; }
        public int Quantity { get; set; }
        public List<Itemcustomfieldvalue2> ItemCustomFieldValues { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string GoodsType { get; set; }
    }

    public class Storeitemreference
    {
        public string Id { get; set; }
        public string ReferenceData { get; set; }
        public string Version { get; set; }
    }

    public class Itemcustomfieldvalue
    {
        public string CustomFieldId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class Storeitemreference1
    {
        public string Id { get; set; }
        public string ReferenceData { get; set; }
        public string Version { get; set; }
    }

    public class Itemcustomfieldvalue1
    {
        public string CustomFieldId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class Storeitemreference2
    {
        public string Id { get; set; }
        public string ReferenceData { get; set; }
        public string Version { get; set; }
    }

    public class Itemcustomfieldvalue2
    {
        public string CustomFieldId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class Recipient
    {
        public Address Address { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AddressId { get; set; }
        public string ReferenceData { get; set; }
        public int ShipmentNumber { get; set; }
        public int SignatureReleaseType { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ShippingMethodId { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DistributionListId { get; set; }
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

}
