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
    public enum ServiceURI
    {
        [Description("https://connect.sandbox.mimeo.com/2012/02/")]
        SandboxUS = 1,
        [Description("https://connect.sandbox.mimeo.co.uk/2012/02/")]
        SandboxUK = 2,
        [Description("https://connect.mimeo.com/2012/02/")]
        ProductionUS = 3,
        [Description("https://connect.mimeo.co.uk/2012/02/")]
        ProductionUK = 4,
        [Description("https://connect.mimeo.de/2012/02/")]
        ProductionDE = 5,
        [Description("https://connect.mimeo.de/2012/02/")]
        QA3US = 6,
        [Description("http://connect3.qa.mimeo.co.uk/2012/02/")]
        QA3UK = 7,
        [Description("http://connect3.qa.mimeo.de/2012/02/")]
        QA3DE = 8,
        [Description("https://connect2.qa.mimeo.com/2014/02/")]
        QA2US201402 = 9,
        [Description("http://localhost:9100/2014/02/")]
        Local201402 = 10,
        [Description("http://localhost:5687/MimeoConnect/2012/02/")]
        Local201202 = 11,
        [Description("https://connect.sandbox.mimeo.com/2014/02/")]
        SandboxUS2014 = 12,
        [Description("https://connect.mimeo.com/2014/02/")]
        ProductionUS2014 = 13,
        [Description("https://connect.mimeo.co.uk/2014/02/")]
        ProductionUK2014 = 14

    }

    public static class EnumHelper
    {
        public static string GetDescription(Enum en)
        {
            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return en.ToString();
        }

    }

}
