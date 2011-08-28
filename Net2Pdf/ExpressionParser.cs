using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Net2Pdf
{
    internal static class ExpressionParser
    {
        public static string GetPropertyName(Expression e)
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
                    return null; //this means no property was passed in
            }
            else if (e is ConstantExpression)
                return null;
            else
                throw new InvalidOperationException("Can't handle this type of operation");
        }
    }
}
