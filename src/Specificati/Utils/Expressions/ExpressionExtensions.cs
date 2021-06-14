using System;
using System.Linq.Expressions;

namespace Specificati.Utils.Expressions
{
    internal static class ExpressionExtensions
    {
        internal static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression)
            => Expression.Lambda<Func<T, bool>>(Expression.Not(expression.Body), expression.Parameters[0]);
    }
}