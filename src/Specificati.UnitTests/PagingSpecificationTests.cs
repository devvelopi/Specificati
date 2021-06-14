using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace Specificati.UnitTests
{
    public class PagingSpecificationTests
    {
        [Test]
        public void Should_Take() {
            // Arrange
            var paging = new PagingSpecification {Take = 2};
            var collection = new[] {"1", "2", "3", "4"};
            var expectedPaged = new[] {"1", "2"};

            // Act
            var pagedCollection = paging.Apply(collection).ToList();

            // Assert
            pagedCollection.Count.ShouldBe(paging.Take!.Value);
            pagedCollection.ShouldBeEquivalentTo(expectedPaged.ToList());
        }

        [Test]
        public void Should_Skip() {
            // Arrange
            var paging = new PagingSpecification {Skip = 2};
            var collection = new[] {"1", "2", "3", "4"};
            var expectedPaged = new[] {"3", "4"};

            // Act
            var pagedCollection = paging.Apply(collection).ToList();

            // Assert
            pagedCollection.Count.ShouldBe(collection.Length - paging.Skip!.Value);
            pagedCollection.ShouldBeEquivalentTo(expectedPaged.ToList());
        }

        [Test]
        public void Should_Take_And_Skip() {
            // Arrange
            var paging = new PagingSpecification {Take = 2, Skip = 1};
            var collection = new[] {"1", "2", "3", "4"};
            var expectedPaged = new[] {"2", "3"};

            // Act
            var pagedCollection = paging.Apply(collection).ToList();

            // Assert
            pagedCollection.Count.ShouldBe(paging.Take!.Value);
            pagedCollection.ShouldBeEquivalentTo(expectedPaged.ToList());
        }

        [Test]
        public void Should_Ignore_If_Empty() {
            // Arrange
            var paging = new PagingSpecification();
            var collection = new[] {"1", "2", "3", "4"};

            // Act
            var pagedCollection = paging.Apply(collection).ToList();

            // Assert
            pagedCollection.ShouldBeEquivalentTo(collection.ToList());
        }

        [Test]
        public void Should_Apply_To_Enumerable() {
            // Arrange
            var paging = new PagingSpecification {Take = 2, Skip = 1};
            var collection = new[] {"1", "2", "3", "4"};
            var expectedPaged = new[] {"2", "3"};

            // Act
            var pagedCollection = paging.Apply(collection).ToList();

            // Assert
            pagedCollection.ShouldBeEquivalentTo(expectedPaged.ToList());
        }

        [Test]
        public void Should_Apply_To_Queryable() {
            // Arrange
            var paging = new PagingSpecification {Take = 2, Skip = 1};
            var collection = new[] {"1", "2", "3", "4"}.AsQueryable();
            var expectedPaged = new[] {"2", "3"};

            // Act
            var pagedCollection = paging.Apply(collection).ToList();

            // Assert
            pagedCollection.ShouldBeEquivalentTo(expectedPaged.ToList());
        }
    }
}