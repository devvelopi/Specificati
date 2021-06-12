using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace Specificati.UnitTests
{
    public class GenericOrderSpecificationTests
    {
        [Test]
        public void Should_Order() {
            // Arrange
            var unsortedCollection = new TestOrderingModel[] {new(4), new(1), new(3), new(2)};
            var order = new GenericOrderSpecification<TestOrderingModel, long>(e => e.Value);
            var expectedOrderedCollection = new List<TestOrderingModel> {new(1), new(2), new(3), new(4)};

            // Act
            // TODO : Cleanliness : Would like to remove generic parameters
            var orderedCollection = unsortedCollection.OrderBy<TestOrderingModel, long>(order).ToList();

            // Assert
            orderedCollection.ShouldBeEquivalentTo(expectedOrderedCollection);
        }

        private record TestOrderingModel(long Value);
    }
}