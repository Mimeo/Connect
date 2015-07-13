using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Xml.Linq;

namespace Connect.ClientLib.Helpers
{
    public static class NameSpaces
    {
        public static XNamespace ns = "http://schemas.mimeo.com/MimeoConnect/2012/02/StorageService";
        public static XNamespace nsOrder = "http://schemas.mimeo.com/MimeoConnect/2012/02/Orders";
        public static XNamespace nsAccountService = "http://schemas.mimeo.com/MimeoConnect/2012/02/AccountService";
        public static XNamespace nsESLOrder = "http://schemas.mimeo.com/EnterpriseServices/2008/09/OrderService";
        public static XNamespace nsESLStorage = "http://schemas.mimeo.com/EnterpriseServices/2008/09/StorageService";
        public static XNamespace nsi = "http://www.w3.org/2001/XMLSchema-instance";
        public static XNamespace nsDoc = "http://schemas.mimeo.com/dom/3.0/Document.xsd";
    }

}
