using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;

[assembly: InternalsVisibleTo("Net2PdfTests")]
namespace Net2Pdf
{
    internal class PropertyMapper
    {
        private StringFormatterCollection _formatters;

        public PropertyMapper(StringFormatterCollection formatters)
        {
            _formatters = formatters;
        }

        public Dictionary<string, string> GetMappings(object obj)
        {
            Dictionary<string, string> fields = new Dictionary<string, string>();
            GetMappings("", obj, fields);
            return fields;
        }

        private void GetMappings(string namePrefix, object obj, Dictionary<string, string> mappings)
        {
            var objType = obj.GetType();

            //Base Case
            string propertyName = namePrefix;
            if (namePrefix.EndsWith("."))
                propertyName = namePrefix.Remove(namePrefix.Length - 1, 1);

            string vFormat = _formatters.ExecuteFormatters(obj, propertyName, obj);

            if (objType.Namespace.StartsWith("System") || vFormat !=null)
                mappings.Add(propertyName, vFormat);
            else
            {
                foreach (var property in objType.GetProperties())
                {
                    string propName = property.Name;
                    object propValue = property.GetValue(obj, null);

                    if (propValue != null)
                    {
                        Type propValueType = property.PropertyType;

                        if (propValue is ICollection)
                        {
                            int i = 0;
                            foreach (var item in (ICollection)propValue)
                            {
                                GetMappings(namePrefix + propName + "[" + i + "].", item, mappings);
                                i++;
                            }
                        }
                        else if (propValueType.GetProperties().Length > 0 && !propValueType.Namespace.StartsWith("System")) //hack for now
                            GetMappings(namePrefix + propName + ".", propValue, mappings);
                        else if (propValueType.IsEnum)
                        {
                            foreach (var e in propValueType.GetEnumValues())
                            {
                                string[] enums = propValue.ToString().Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

                                bool containsEnum = enums.Contains(e.ToString());

                                if (containsEnum && !mappings.ContainsKey(namePrefix + propName))
                                {
                                    string v = _formatters.ExecuteFormatters(obj, propName, e) ?? e.ToString();
                                    mappings.Add(namePrefix + propName, v);
                                }

                                string f = _formatters.ExecuteFormatters(obj, propName, containsEnum) ?? containsEnum.ToString();
                                mappings.Add(namePrefix + propName + "." + e.ToString(), f);
                            }
                        }
                        else
                        {
                            string v = _formatters.ExecuteFormatters(obj, propName, propValue) ?? propValue.ToString();
                            mappings.Add(namePrefix + propName, v);
                        }
                    }
                }
            }
        }
    }
}
