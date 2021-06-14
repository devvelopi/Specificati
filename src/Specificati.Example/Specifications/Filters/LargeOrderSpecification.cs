using System;
using System.Linq.Expressions;
using Specificati.Example.Models;

namespace Specificati.Example.Specifications.Filters
{
    public class LargeOrderSpecification : FilterSpecification<Order>
    {
        public override Expression<Func<Order, bool>> FilterExpression => o => o.Amount > 10;
    }
}