using System;
using System.Collections.Generic;
using System.Linq;
using Specificati.Example.Dtos;
using Specificati.Example.Models;
using Specificati.Example.Specifications.Projections;

namespace Specificati.Example
{
    public class Example
    {
        private static readonly List<Order> Orders = new List<Order>()
        {
            new Order
            {
                Name = "Shoes",
                Amount = 10,
                ProcessingDate = DateTime.UtcNow.AddDays(-14)
            },
            new Order
            {
                Name = "Jackets",
                Amount = 15,
                ProcessingDate = DateTime.UtcNow.AddDays(-5)
            },
            new Order
            {
                Name = "Pants",
                Amount = 2,
                ProcessingDate = DateTime.UtcNow
            }
        };
        
        public static void FilterOrderProject()
        {
            Orders
                .Where(Order.Recent & (Order.Large | Order.WithName("Pants")))
                .OrderBy<Order, DateTime>(Order.ByProcessingDate)
                .Select<Order, OrderDto>(new OrderToOrderDtoProjectionSpecification())
                .ToList();
        }

        public static void Satisfaction()
        {
            var order = Orders.First();

            var specification = Order.Recent & !Order.Large;
            var satisfied = specification.IsSatisfiedBy(order);
        }

        public static void Paging()
        {
            var pagination = new PagingSpecification {Skip = 5, Take = 15};
            var paginated = pagination.Apply(Orders);
        }
    }
}