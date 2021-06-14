using System;
using System.Linq.Expressions;
using Specificati.Example.Dtos;
using Specificati.Example.Models;

namespace Specificati.Example.Specifications.Projections
{
    public class OrderToOrderDtoProjectionSpecification : ProjectionSpecification<Order, OrderDto>
    {
        public override Expression<Func<Order, OrderDto>> ProjectionExpression => o => new OrderDto
        {
            Date = o.ProcessingDate,
            Name = o.Name,
            TotalAmount = o.Amount
        };
    }
}