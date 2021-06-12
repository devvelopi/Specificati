using System;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;
using Shouldly;

namespace Specificati.UnitTests
{
    public class FilterSpecificationTests
    {
        [Test]
        public void Should_Satisfy() {
            // Arrange
            var model = new TestFilterModel("NAME");
            var specification = new TestValueEqualsFilterSpecification("NAME");

            // Act
            var satisfied = specification.IsSatisfiedBy(model);

            // Assert
            satisfied.ShouldBeTrue(
                $"the value '{model.Value}' should satisfy ({specification.Value}=={model.Value})"
            );
        }

        [Test]
        public void Should_Not_Satisfy() {
            // Arrange
            var model = new TestFilterModel("NOT");
            var specification = new TestValueEqualsFilterSpecification("NAME");

            // Act
            var satisfied = specification.IsSatisfiedBy(model);

            // Assert
            satisfied.ShouldBeFalse(
                $"the value '{model.Value}' should not satisfy ({specification.Value}=={model.Value})"
            );
        }

        [Test]
        public void Should_And() {
            // Arrange
            var models = new TestFilterModel[] {new("NAME"), new("A"), new("AME")};
            var combinedSpecification =
                new TestValueEqualsFilterSpecification("NAME") & new TestValueContainsFilterSpecification("A");

            // Act
            var filteredSet = models.Where(combinedSpecification).ToList();

            // Assert
            filteredSet.Count.ShouldBe(1);
            filteredSet.Single().Value.ShouldBe("NAME");
        }

        [Test]
        public void Should_Or() {
            // Arrange
            var models = new TestFilterModel[] {new("NAME"), new("A"), new("AME")};
            var combinedSpecification =
                new TestValueEqualsFilterSpecification("NAME") | new TestValueContainsFilterSpecification("A");

            // Act
            var filteredSet = models.Where(combinedSpecification).ToList();

            // Assert
            filteredSet.Count.ShouldBe(3);
            filteredSet.ShouldBeEquivalentTo(models.ToList());
        }

        [Test]
        public void Should_Not() {
            // Arrange
            var models = new TestFilterModel[] {new("NAME"), new("A"), new("AME")};
            var specification = new TestValueContainsFilterSpecification("A");

            // Act
            var filteredSet = models.Where(!specification).ToList();

            // Assert
            filteredSet.Count.ShouldBe(0);
        }

        [Test]
        public void Should_Implicitly_Cast_To_Expression() {
            // Arrange
            var specification = new TestValueContainsFilterSpecification("A");

            // Act
            Expression<Func<TestFilterModel, bool>> expression = specification;

            // Assert
            expression.ShouldNotBeNull();
        }

        [Test]
        public void Should_Implicitly_Cast_To_Func() {
            // Arrange
            var specification = new TestValueContainsFilterSpecification("A");

            // Act
            Func<TestFilterModel, bool> func = specification;

            // Assert
            func.ShouldNotBeNull();
        }

        private record TestFilterModel(string Value);

        private class TestValueEqualsFilterSpecification : FilterSpecification<TestFilterModel>
        {
            public TestValueEqualsFilterSpecification(string value) { Value = value; }

            public string Value { get; }

            public override Expression<Func<TestFilterModel, bool>> FilterExpression => e => e.Value == Value;
        }

        private class TestValueContainsFilterSpecification : FilterSpecification<TestFilterModel>
        {
            public TestValueContainsFilterSpecification(string value) { Value = value; }

            public string Value { get; }

            public override Expression<Func<TestFilterModel, bool>> FilterExpression => e => e.Value.Contains(Value);
        }
    }
}