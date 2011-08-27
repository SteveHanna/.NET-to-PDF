using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Net2Pdf
{
    public class ExpressionFormatter<T> : IStringFormatter
    {
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
                    var propName = GetPropertyName((Expression)expression);
                    if (propertyName == propName)
                        return expression.Compile()((T)obj);
                }
                else if (propertyValue.GetType() == typeof(T))
                    return expression.Compile()((T)propertyValue);
            }

            return null;
        }

        private static string GetPropertyName(Expression e)
        {
            if (e is LambdaExpression)
                return GetPropertyName(((LambdaExpression)e).Body);
            else if (e is BinaryExpression)
            {
                var be = (BinaryExpression)e;

                string left = GetPropertyName(be.Left);
                if (left != null)
                    return left;
                var right = GetPropertyName(be.Right);
                if (right != null)
                    return right;

                return null;
            }
            else if (e is UnaryExpression)
            {
                return GetPropertyName((e as UnaryExpression).Operand);
            }
            else if (e is MemberExpression)
            {
                return (e as MemberExpression).Member.Name;
            }
            else if (e is MethodCallExpression)
            {
                var x = (e as MethodCallExpression);
                if (x.Object is MemberExpression)
                    return GetPropertyName(x.Object);
                else
                    return null;
            }
            else
                throw new InvalidOperationException("Can't handle this type of operation");
        }
    }
}
