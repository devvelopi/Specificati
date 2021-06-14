using System;
using System.Linq.Expressions;
using Specificati.Example.Models;

namespace Specificati.Example.Specifications.Filters
{
    public class RecentOrderSpecification : FilterSpecification<Order>
    {
        public override Expression<Func<Order, bool>> FilterExpression =>
            o => o.ProcessingDate <= DateTime.UtcNow.AddDays(-7);
    }
}