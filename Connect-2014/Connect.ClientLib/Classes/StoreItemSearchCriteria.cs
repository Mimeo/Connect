using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Connect.ClientLib
{

    public class PageInfo
    {
        public long PageSize { get; set; }
        public long PageNumber { get; set; }
    }

    public class SortBy
    {
        public int Property { get; set; }
        public int Direction { get; set; }
    }

    public class Created
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class LastOrdered
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class LastUpdated
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class DocumentFile
    {
        public string FileId { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastModified { get; set; }
        public DateTime LastUsed { get; set; }
    }

    public class ItemDetails
    {
        public DocumentFile DocumentFile { get; set; }
    }

    public class NumberOfTimesUsedInAnOrder
    {
        public long Min { get; set; }
        public long Max { get; set; }
    }

    public class StoreItemSearchCriteria 
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PageInfo PageInfo { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<SortBy> SortBy { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SearchString { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Created Created { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CreatedBy { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public LastOrdered LastOrdered { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public LastUpdated LastUpdated { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FolderId { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ReferenceData { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int LevelOfDetail { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool IncludeSubFolders { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int FolderScope { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OfferingCode { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FolderOwnerUserId { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ItemDetails ItemDetails { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public NumberOfTimesUsedInAnOrder NumberOfTimesUsedInAnOrder { get; set; }
    }
}
