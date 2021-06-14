using System;
using System.Linq.Expressions;
using Specificati.Example.Models;

namespace Specificati.Example.Specifications.Orders
{
    public class OrderByProcessingDateSpecification : OrderSpecification<Order, DateTime>
    {
        public override Expression<Func<Order, DateTime>> OrderExpression => o => o.ProcessingDate;
    }
}