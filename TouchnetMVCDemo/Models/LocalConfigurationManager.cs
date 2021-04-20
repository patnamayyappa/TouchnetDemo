using Cmc.Core.Configuration;
using System;
using System.Collections.Specialized;
using System.Configuration;

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