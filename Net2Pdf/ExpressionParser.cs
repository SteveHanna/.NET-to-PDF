using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Net2Pdf
{
    internal static class ExpressionParser
    {
        public static string FindPropertyNameInExpression(Expression e)
        {
            if (e.NodeType == ExpressionType.Lambda)
                return FindPropertyNameInExpression(((LambdaExpression)e).Body);
            else if (e is BinaryExpression)
            {
                var be = (BinaryExpression)e;

                string left = FindPropertyNameInExpression(be.Left);
                if (left != null)
                    return left;
                var right = FindPropertyNameInExpression(be.Right);
                if (right != null)
                    return right;

                return null;
            }
            else if (e is UnaryExpression)
                return FindPropertyNameInExpression((e as UnaryExpression).Operand);
            else if (e is MemberExpression)
                return (e as MemberExpression).Member.Name;
            else if (e is MethodCallExpression)
            {
                var x = (MethodCallExpression)e;
                if (x.Object is MemberExpression)
                    return FindPropertyNameInExpression(x.Object);
                else
                    return null; //this means no property was part of the method
            }
            else if (e is ConstantExpression)
                return null;
            else
                throw new InvalidOperationException("Can't handle this type of operation");
        }
    }
}
