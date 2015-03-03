using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using SomeBasicNEventStoreApp.Core;

namespace SomeBasicNEventStoreApp.Tests
{
    public class XmlImport
    {
        public static object Parse(XElement target, Type type, XNamespace ns)
        {
            var props = type.GetProperties();
            var customerObj = Activator.CreateInstance(type);
            foreach (var propertyInfo in props)
            {
                XElement propElement = target.Element(ns + propertyInfo.Name);
                if (null != propElement)
                {
                    if (!(propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType == typeof(string)))
                    {
                        Console.WriteLine("ignoring {0} {1}", type.Name, propertyInfo.PropertyType.Name);
                    }
                    else
                    {
                        var value = Convert.ChangeType(propElement.Value, propertyInfo.PropertyType, CultureInfo.InvariantCulture.NumberFormat);
                        propertyInfo.SetValue(customerObj, value, null);
                    }
                }
            }
            return customerObj;
        }

        public static void Parse(XDocument xDocument, IEnumerable<Type> types, Action<Type, Object> onParsedEntity, XNamespace ns)
        {
            var db = xDocument.Root;

            foreach (var type in types)
            {
                var elements = db.Elements(ns + type.Name);

                foreach (var element in elements)
                {
                    var obj = Parse(element, type, ns);
                    onParsedEntity(type, obj);
                }
            }
        }
    }
}
