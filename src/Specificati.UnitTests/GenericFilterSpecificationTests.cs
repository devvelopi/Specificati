using NUnit.Framework;
using Shouldly;

namespace Specificati.UnitTests
{
    public class GenericFilterSpecificationTests
    {
        [Test]
        public void Should_Satisfy() {
            // Arrange
            var model = new TestFilterModel("NAME");
            var specification = new GenericFilterSpecification<TestFilterModel>(e => e.Value.Contains("A"));

            // Act
            var satisfied = specification.IsSatisfiedBy(model);

            // Assert
            satisfied.ShouldBeTrue($"the value '{model.Value}' should satisfy the expression");
        }

        private record TestFilterModel(string Value);
    }
}