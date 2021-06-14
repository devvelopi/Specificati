using System;

namespace Specificati.Example.Dtos
{
    public class OrderDto
    {
        public string Name { get; set; }
        public long TotalAmount { get; set; }
        public DateTime Date { get; set; }
    }
}