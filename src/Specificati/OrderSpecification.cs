using System;
using System.Linq.Expressions;

namespace Specificati
{
    public abstract class OrderSpecification<T, TKey>
    {
        public abstract Expression<Func<T, TKey>> OrderExpression { get; }

        public static implicit operator Expression<Func<T, TKey>>(OrderSpecification<T, TKey> orderSpecification) =>
            orderSpecification.OrderExpression;

        public static implicit operator Func<T, TKey>(OrderSpecification<T, TKey> orderSpecification) =>
            orderSpecification.OrderExpression.Compile();
    }
}