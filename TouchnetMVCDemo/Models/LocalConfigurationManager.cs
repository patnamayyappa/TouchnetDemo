using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;
using Cmc.Core.Configuration;

namespace TouchnetMVCDemo.Models
{
    public class LocalConfigurationManager : IConfigurationManager
    {
        public T GetSection<T>(string sectionName)
        {
            throw new NotImplementedException();
        }

        public NameValueCollection AppSettings { get; }
        public ConnectionStringSettingsCollection ConnectionStrings { get; }
    }
}