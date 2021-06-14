using System.Collections.Generic;
using System.Linq;

namespace Specificati
{
    public record PagingSpecification
    {
        public int? Skip { get; set; }
        public int? Take { get; set; }

        public IQueryable<T> Apply<T>(IQueryable<T> query) {
            var compiledQuery = query;
            if (Skip != null) compiledQuery = compiledQuery.Skip(Skip.Value);
            if (Take != null) compiledQuery = compiledQuery.Take(Take.Value);
            return compiledQuery;
        }

        public IEnumerable<T> Apply<T>(IEnumerable<T> query) {
            var compiledQuery = query;
            if (Skip != null) compiledQuery = compiledQuery.Skip(Skip.Value);
            if (Take != null) compiledQuery = compiledQuery.Take(Take.Value);
            return compiledQuery;
        }
    }
}