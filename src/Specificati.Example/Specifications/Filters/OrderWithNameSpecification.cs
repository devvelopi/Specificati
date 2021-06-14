using System;
using System.Linq.Expressions;
using Specificati.Example.Models;

namespace Specificati.Example.Specifications.Filters
{
    public class OrderWithNameSpecification : FilterSpecification<Order>
    {
        private readonly string _name;

        public OrderWithNameSpecification(string name)
        {
            _name = name;
        }

        public override Expression<Func<Order, bool>> FilterExpression => o => o.Name == _name;
    }
}