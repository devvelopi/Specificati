using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;
using Shouldly;

namespace Specificati.UnitTests
{
    public class OrderSpecificationTests
    {
        private static TestOrderSpecification Order => new();

        [Test]
        public void Should_Order() {
            // Arrange
            var unsortedCollection = new TestOrderingModel[] {new(4), new(1), new(3), new(2)};
            var expectedOrderedCollection = new List<TestOrderingModel> {new(1), new(2), new(3), new(4)};

            // Act
            // TODO : Cleanliness : Would like to remove generic parameters
            var orderedCollection = unsortedCollection.OrderBy<TestOrderingModel, long>(Order).ToList();

            // Assert
            orderedCollection.ShouldBeEquivalentTo(expectedOrderedCollection);
        }

        [Test]
        public void Should_Implicitly_Cast_To_Expression() {
            // Arrange
            var unsortedCollection = new TestOrderingModel[] {new(4), new(1), new(3), new(2)}.AsQueryable();
            var expectedOrderedCollection = new List<TestOrderingModel> {new(1), new(2), new(3), new(4)};

            // Act
            // TODO : Cleanliness : Would like to remove generic parameters
            var orderedCollection = unsortedCollection.OrderBy<TestOrderingModel, long>(Order).ToList();

            // Assert
            orderedCollection.ShouldBeEquivalentTo(expectedOrderedCollection);
        }

        private record TestOrderingModel(long Value);

        private class TestOrderSpecification : OrderSpecification<TestOrderingModel, long>
        {
            public override Expression<Func<TestOrderingModel, long>> OrderExpression => e => e.Value;
        }
    }
}