using Specificati.Example.Specifications.Filters;
using Specificati.Example.Specifications.Orders;

namespace Specificati.Example.Models
{
    public partial class Order
    {
        public static RecentOrderSpecification Recent => new RecentOrderSpecification();
        public static LargeOrderSpecification Large => new LargeOrderSpecification();
        public static OrderWithNameSpecification WithName(string name) => new OrderWithNameSpecification(name);
        
        public static OrderByProcessingDateSpecification ByProcessingDate => new OrderByProcessingDateSpecification();
    }
}