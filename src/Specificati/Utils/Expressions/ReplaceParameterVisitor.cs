using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Specificati.Utils.Expressions
{
    internal class ReplaceParameterVisitor : ExpressionVisitor,
        IEnumerable<KeyValuePair<ParameterExpression, ParameterExpression>>
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> _parameterMappings = new();

        public IEnumerator<KeyValuePair<ParameterExpression, ParameterExpression>> GetEnumerator() =>
            _parameterMappings.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        protected override Expression VisitParameter(ParameterExpression node)
            => _parameterMappings.TryGetValue(node, out var newValue) ? newValue : node;

        public void Add(ParameterExpression parameterToReplace, ParameterExpression replaceWith) =>
            _parameterMappings.Add(parameterToReplace, replaceWith);
    }
}