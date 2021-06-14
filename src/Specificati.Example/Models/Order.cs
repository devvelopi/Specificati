using System;

namespace Specificati.Example.Models
{
    public partial class Order
    {
        public string Name { get; set; }
        public long Amount { get; set; }
        public DateTime ProcessingDate { get; set; }
    }
}