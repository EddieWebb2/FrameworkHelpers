using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using FrameworkHelpers.Common.Converters;
using FrameworkHelpers.Utilities.Models;

namespace FrameworkHelpers.Utilities
{
    public static class AppConfig
    {
        private static readonly List<AppConfigSource> _sources;

        static AppConfig()
        {
            _sources = new List<AppConfigSource>();

            var databaseSuffixes = new[] { "DB", "ConnectionString", "Connection" };

            _sources.Add(new AppConfigSource
            {
                CanHandle = name => databaseSuffixes.Any(name.EndsWith) && ConfigurationManager.ConnectionStrings[name] != null,
                Value = name => ConfigurationManager.ConnectionStrings[name].ConnectionString
            });
            
            _sources.Add(new AppConfigSource
            {
                CanHandle = name => true,
                Value = name => ConfigurationManager.AppSettings[name] ?? string.Empty
            });
        }

        public static void FromAppConfig(this object config)
        {
            var properties = config
                .GetType()
                .GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite);

            foreach (var property in properties)
            {
                var value = _sources.First(source => source.CanHandle(property.Name)).Value(property.Name);

                TypeConverter.SetProperty(
                    config,
                    property,
                    value);
            }
        }
    }
}
