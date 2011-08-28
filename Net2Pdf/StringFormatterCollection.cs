using System;
using System.Collections.Generic;

namespace Net2Pdf
{
    internal class StringFormatterCollection : IEnumerable<IStringFormatter>
    {
        private List<IStringFormatter> _formatters = new List<IStringFormatter>();

        public void Add(IStringFormatter formatter)
        {
            _formatters.Add(formatter);
        }

        public string ExecuteFormatters(object obj, string propName, object propValue)
        {
            string propValueString = null;

            foreach (var formatter in this)
            {
                propValueString = formatter.FormatString(obj, propName, propValue);
                if (propValueString != null)
                    return propValueString;
            }

            //if (propValueString == null)
            //    propValueString = propValue.ToString();

            return propValueString;
        }

        public IEnumerator<IStringFormatter> GetEnumerator()
        {
            return _formatters.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _formatters.GetEnumerator();
        }
    }
}
