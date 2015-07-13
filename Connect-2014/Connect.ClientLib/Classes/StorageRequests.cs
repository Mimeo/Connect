using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Connect.ClientLib
{
    public class GetTopLevelFoldersRequest  
    {
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int LevelOfDetail { get; set; }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Scope { get; set; }
        
    }

    public class GetFolderRequest 
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FolderId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int LevelOfDetail { get; set; }

    }
}
