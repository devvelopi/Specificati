using System;
using System.Linq.Expressions;

namespace Specificati
{
    public sealed class GenericOrderSpecification<T, TKey> : OrderSpecification<T, TKey>
    {
        public GenericOrderSpecification(Expression<Func<T, TKey>> order) { OrderExpression = order; }

        public override Expression<Func<T, TKey>> OrderExpression { get; }
    }
}