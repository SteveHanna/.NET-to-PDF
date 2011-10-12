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
        private object _originalObj;

        public PropertyMapper(StringFormatterCollection formatters)
        {
            _formatters = formatters;
        }

        public Dictionary<string, string> GetMappings(object obj)
        {
            _originalObj = obj;
            Dictionary<string, string> fields = new Dictionary<string, string>();
            GetMappings(string.Empty, obj, fields);
            return fields;
        }

        private void GetMappings(string namePrefix, object obj, Dictionary<string, string> mappings)
        {
            //Base Case
            string vFormat = _formatters.ExecuteFormatters(_originalObj, namePrefix.Split('.').Last(), obj);
            if (obj is ValueType || obj is string || vFormat != null)
                mappings.Add(namePrefix.TrimEnd('.'), vFormat ?? obj.ToString());
            else
            {
                foreach (var property in obj.GetType().GetProperties())
                {
                    string propName = property.Name;
                    object propValue = property.GetValue(obj, null);
                    if (propValue == null) break;

                    if (propValue is IEnumerable && propValue is string == false)
                    {
                        int i = 0;
                        foreach (var item in (IEnumerable)propValue)
                        {
                            GetMappings(namePrefix + propName + "[" + i + "].", item, mappings);
                            i++;
                        }
                    }
                    else if (propValue is ValueType == false && propValue is string == false)
                        GetMappings(namePrefix + propName + ".", propValue, mappings);
                    else if (propValue is Enum)
                        HandleEnums(namePrefix + propName, propValue, mappings);
                    else
                        GetMappings(namePrefix + propName, propValue, mappings);
                }
            }
        }

        private void HandleEnums(string name, object value, Dictionary<string, string> mappings)
        {
            if (value is Enum)
            {
                foreach (var e in (value.GetType().GetEnumValues()))
                {
                    string[] enums = value.ToString().Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                    bool containsEnum = enums.Contains(e.ToString());

                    if (containsEnum && !mappings.ContainsKey(name))
                        mappings.Add(name, _formatters.ExecuteFormatters(_originalObj, name, e) ?? e.ToString());

                    string f = _formatters.ExecuteFormatters(_originalObj, name, containsEnum) ?? containsEnum.ToString();
                    mappings.Add(name + "." + e.ToString(), f);
                }
            }
        }

    }
}