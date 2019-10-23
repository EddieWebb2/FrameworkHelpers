using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FrameworkHelpers.Common.Converters
{
    public class TypeConverter
    {
        private static readonly List<Conversion> Converters;

        static TypeConverter()
        {
            Converters = new List<Conversion>
            {
                Conversion.ForType(value => new Uri(value)),
                Conversion.ForType(int.Parse),
                Conversion.ForType(double.Parse),
                Conversion.ForType(decimal.Parse),
                Conversion.ForType(TimeSpan.Parse),
                Conversion.ForType(bool.Parse),
                Conversion.ForType(Guid.Parse),
                new Conversion {
                    CanConvert = t => t.GetInterface("IEnumerable") != null && t.GetGenericArguments().Any(i=> i.IsEnum),
                    Convert = (type, value) =>
                    {
                        var t = type.GetGenericArguments()[0];
                        var list = typeof(List<>).MakeGenericType(t);
                        IList lists = (IList)Activator.CreateInstance(list);
                        foreach (var item in value.Split(','))
                        {
                            lists.Add(Convert.ChangeType(Enum.Parse(t, item.Trim()), t));
                        }
                        return lists;
                    }
                },
                new Conversion
                {
                    CanConvert = t => t.IsEnum,
                    Convert = (type, value) => Enum.Parse(type,value)
                },
                Conversion.ForType(value => value.Split(',').Select(s => s.Trim())),
                Conversion.ForType(value => new HashSet<string>(value.Split(',').Select(s => s.Trim()))),

                //must be last
                new Conversion { CanConvert = t => true, Convert = (type, value) => value }
            };
        }

        public static void RegisterConverter<T>(Func<string, object> converter)
        {
            var conversion = new Conversion
            {
                CanConvert = t => t == typeof(T),
                Convert = (type, value) => converter(value)
            };

            Converters.Insert(Converters.Count - 1, conversion);
        }

        public static void SetProperty(object host, PropertyInfo property, string value)
        {
            try
            {
                var convertedValue = ConvertTo(property.PropertyType, value);
                var setter = property.GetSetMethod(true);

                setter.Invoke(host, new[] { convertedValue });
            }
            catch (FormatException e)
            {
                // throw meaningful error message when field can't be converted
                throw new FormatException($"{property.Name} could not be converted due to {e.Message}");
            }
        }

        private static object ConvertTo(Type type, string value)
        {
            return Converters.First(c => c.CanConvert(type)).Convert(type, value);
        }

        private class Conversion
        {
            public Func<Type, bool> CanConvert { get; set; }
            public Func<Type, string, object> Convert { get; set; }

            public static Conversion ForType<T>(Func<string, T> convert)
            {
                return new Conversion
                {
                    CanConvert = t => t == typeof(T),
                    Convert = (type, value) => convert(value)
                };
            }

        }
    }
}
