using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect.ClientLib
{

    public class ShippingMethodDetail
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class DeliveryOptionQuote
    {
        public ShippingMethodDetail ShippingMethodDetail { get; set; }
        public DateTime DeliveryCommitmentDate { get; set; }
        public double DeliveryCharge { get; set; }
    }

    public class AvailableDeliveryOptionsPerRecipient
    {
        public string RecipientId { get; set; }
        public string RecipientReferenceData { get; set; }
        public List<DeliveryOptionQuote> DeliveryOptionQuotes { get; set; }
    }

    public class OrderAvailableDeliveryOptions 
    {
        public int AdditionalProcessingHoursAllowed { get; set; }
        public DateTime ShipDate { get; set; }
        public List<AvailableDeliveryOptionsPerRecipient> AvailableDeliveryOptionsPerRecipient { get; set; }

        public Guid findDeliveryOption(string name){
            Guid retId = new Guid();

            string nameId = this.AvailableDeliveryOptionsPerRecipient[0].DeliveryOptionQuotes.Find(x => x.ShippingMethodDetail.Name == name).ShippingMethodDetail.Id;

            if (Guid.TryParse(nameId, out retId))
                return retId;
            else
                return Guid.Empty;
        }
    }
}
