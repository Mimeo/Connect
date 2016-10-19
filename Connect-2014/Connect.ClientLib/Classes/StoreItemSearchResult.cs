using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Connect.ClientLib
{

    public class StoreItemSearchResults
    {

        public class SearchResults
        {
            public int TotalItemCount { get; set; }
            public Item[] Items { get; set; }
        }

        public class Item
        {
            public string Id { get; set; }
            public Folder Folder { get; set; }
            public DateTime Created { get; set; }
            public DateTime LastModified { get; set; }
            public int Version { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string SystemDetails { get; set; }
            public string ReferenceData { get; set; }
            public Lastmodifiedbyuserdetails LastModifiedByUserDetails { get; set; }
            public string ItemData { get; set; }
            public Createdbyuserdetails CreatedByUserDetails { get; set; }
            public Itemdetails ItemDetails { get; set; }
            public string Type { get; set; }
            public string GoodsType { get; set; }
        }

        public class Folder
        {
            public string Id { get; set; }
            public string ParentFolderId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public int Attributes { get; set; }
            public string ReferenceData { get; set; }
            public DateTime Created { get; set; }
            public Owneruserdetails OwnerUserDetails { get; set; }
            public int Permissions { get; set; }
        }

        public class Owneruserdetails
        {
            public string Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
        }

        public class Lastmodifiedbyuserdetails
        {
            public string Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
        }

        public class Createdbyuserdetails
        {
            public string Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
        }

        public class Itemdetails
        {
            public string __type { get; set; }
            public string StoreItemId { get; set; }
            public int StoreItemVersion { get; set; }
            public int BlackAndWhiteImpressionCount { get; set; }
            public int ColorImpressionCount { get; set; }
            public int TabCount { get; set; }
            public int SlipSheetCount { get; set; }
            public int SheetCount { get; set; }
            public int PrintColor { get; set; }
            public int Plex { get; set; }
            public string SpecialInstructionList { get; set; }
            public string ProductTypeId { get; set; }
            public string ProductSizeId { get; set; }
            public int Attributes { get; set; }
            public int EverydayColorImpressionCount { get; set; }
        }

    }
}
