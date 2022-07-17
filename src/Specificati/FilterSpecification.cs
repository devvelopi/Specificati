using System;
using System.Linq;
using System.Linq.Expressions;
using Specificati.Utils.Expressions;

namespace Specificati
{
    public abstract class FilterSpecification<T>
    {
        public abstract Expression<Func<T, bool>> FilterExpression { get; }

        public virtual bool IsSatisfiedBy(T item) => FilterExpression.Compile().Invoke(item);

        public static implicit operator Expression<Func<T, bool>>(FilterSpecification<T> filterSpecification) =>
            filterSpecification.FilterExpression;

        public static implicit operator Func<T, bool>(FilterSpecification<T> filterSpecification) =>
            filterSpecification.FilterExpression.Compile();

        public static FilterSpecification<T> operator &(FilterSpecification<T> left, FilterSpecification<T> right) =>
            CombineSpecification(left, right, Expression.AndAlso);

        public static FilterSpecification<T> operator |(FilterSpecification<T> left, FilterSpecification<T> right) =>
            CombineSpecification(left, right, Expression.Or);

        public static FilterSpecification<T> operator !(FilterSpecification<T> filterSpecification) =>
            new ConstructedFilterSpecification<T>(filterSpecification.FilterExpression.Not());

        private static FilterSpecification<T> CombineSpecification(FilterSpecification<T> left,
            FilterSpecification<T> right,
            Func<Expression, Expression, BinaryExpression> combiner
        ) {
            var expr1 = left.FilterExpression;
            var expr2 = right.FilterExpression;
            var arg = Expression.Parameter(typeof(T));
            var combined = combiner.Invoke(
                new ReplaceParameterVisitor {{expr1.Parameters.Single(), arg}}.Visit(expr1.Body),
                new ReplaceParameterVisitor {{expr2.Parameters.Single(), arg}}.Visit(expr2.Body)
            );
            return new ConstructedFilterSpecification<T>(
                Expression.Lambda<Func<T, bool>>(combined, arg)
            );
        }

        private class ConstructedFilterSpecification<TType> : FilterSpecification<TType>
        {
            public ConstructedFilterSpecification(Expression<Func<TType, bool>> expression) {
                FilterExpression = expression;
            }

            public override Expression<Func<TType, bool>> FilterExpression { get; }
        }
    }
}
