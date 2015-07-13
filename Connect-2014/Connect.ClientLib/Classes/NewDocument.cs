using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect.ClientLib
{
    public class NewDocument
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ReferenceData { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Product Product { get; set; }
    }

    public class Product
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ApplicationId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Quantity { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Template { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DocumentTemplateId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DocumentTemplateName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Connect.ClientLib.Template.Content[] Content { get; set; }
    }
}
