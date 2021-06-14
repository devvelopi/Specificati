using System;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;
using Shouldly;

namespace Specificati.UnitTests
{
    public class ProjectionSpecificationTests
    {
        private static TestProjectionSpecification Projection => new();

        [Test]
        public void Should_Apply_To_Single() {
            // Arrange
            var model = new TestProjectionModel("Value");

            // Act
            var projected = Projection.Apply(model);

            // Assert
            projected.OtherValue.ShouldBe(model.Value);
        }

        [Test]
        public void Should_Implicitly_Convert_To_Expression() {
            // Arrange
            var models = new TestProjectionModel[] {new("Value1"), new("Value2"), new("Value3")}.AsQueryable();

            // Act
            // TODO : Cleanliness : Would like this to not have to include generic parameters
            var projected = models.Select<TestProjectionModel, TestProjectedModel>(Projection).ToList();

            // Assert
            projected.Count.ShouldBe(models.Count());
            projected.ShouldAllBe(p => models.Any(m => m.Value == p.OtherValue));
        }

        [Test]
        public void Should_Implicitly_Convert_To_Func() {
            // Arrange
            var models = new TestProjectionModel[] {new("Value1"), new("Value2"), new("Value3")};

            // Act
            // TODO : Cleanliness : Would like this to not have to include generic parameters
            var projected = models.Select<TestProjectionModel, TestProjectedModel>(Projection).ToList();

            // Assert
            projected.Count.ShouldBe(models.Length);
            projected.ShouldAllBe(p => models.Any(m => m.Value == p.OtherValue));
        }

        private record TestProjectionModel(string Value);

        private record TestProjectedModel(string OtherValue);

        private class TestProjectionSpecification : ProjectionSpecification<TestProjectionModel, TestProjectedModel>
        {
            public override Expression<Func<TestProjectionModel, TestProjectedModel>> ProjectionExpression =>
                from => new(from.Value);
        }
    }
}