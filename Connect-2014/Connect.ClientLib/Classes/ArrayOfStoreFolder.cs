using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect.ClientLib
{

    public class ArrayOfStoreFolder
    {

        public class TopFolder
        {
            public Folder[] folder { get; set; }
        }

        public class Folder
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public int Attributes { get; set; }
            public string ReferenceData { get; set; }
            public DateTime Created { get; set; }
            public Subfolder[] SubFolders { get; set; }
            public int Permissions { get; set; }
            public int WellKnownFolderType { get; set; }
        }

        public class Subfolder
        {
            public string Id { get; set; }
            public string ParentFolderId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public int Attributes { get; set; }
            public string ReferenceData { get; set; }
            public DateTime Created { get; set; }
            public Owneruserdetails OwnerUserDetails { get; set; }
            public object[] SubFolders { get; set; }
            public int Permissions { get; set; }
        }

        public class Owneruserdetails
        {
            public string Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
        }


    }
}
