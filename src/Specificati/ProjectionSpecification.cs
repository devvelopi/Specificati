using System;
using System.Linq.Expressions;

namespace Specificati
{
    public abstract class ProjectionSpecification<TFrom, TTo>
    {
        public abstract Expression<Func<TFrom, TTo>> ProjectionExpression { get; }
        public Func<TFrom, TTo> ProjectionFunc => ProjectionExpression.Compile();

        public virtual TTo Apply(TFrom from) => ProjectionExpression.Compile()(from);

        public static implicit operator Expression<Func<TFrom, TTo>>(ProjectionSpecification<TFrom, TTo> spec) =>
            spec.ProjectionExpression;

        public static implicit operator Func<TFrom, TTo>(ProjectionSpecification<TFrom, TTo> spec) =>
            spec.ProjectionExpression.Compile();
    }
}