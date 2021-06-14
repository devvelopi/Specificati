using System;
using System.Linq.Expressions;

namespace Specificati
{
    public sealed class GenericFilterSpecification<T> : FilterSpecification<T>
    {
        public GenericFilterSpecification(Expression<Func<T, bool>> expression) { FilterExpression = expression; }

        public override Expression<Func<T, bool>> FilterExpression { get; }
    }
}