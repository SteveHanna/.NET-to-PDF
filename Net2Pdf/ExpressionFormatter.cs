using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Net2Pdf
{
    internal class ExpressionFormatter<T> : IStringFormatter
    {
        public Expression<Func<T, string>>[] Expressions { get; set; }

        private Expression<Func<T, string>>[] expressions;

        public ExpressionFormatter(params Expression<Func<T, string>>[] e)
        {
            expressions = e;
        }

        public string FormatString(object obj, string propertyName, object propertyValue)
        {
            foreach (var expression in expressions)
            {
                if (obj.GetType() == typeof(T))
                {
                    var propName = ExpressionParser.GetPropertyName((Expression)expression);
                    if (propertyName == propName || propName == null)
                        return expression.Compile()((T)obj);
                }
                else if (propertyValue.GetType() == typeof(T))
                    return expression.Compile()((T)propertyValue);
            }

            return null;
        }
    }
}
