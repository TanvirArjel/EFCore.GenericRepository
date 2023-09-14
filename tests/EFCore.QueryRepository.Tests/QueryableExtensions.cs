using System.Collections;
using System.Linq.Expressions;

namespace EFCore.QueryRepository.Tests;

public static class QueryableExtensions
{
    public static IQueryable<T> AsAsyncQueryable<T>(this IEnumerable<T> input)
    {
        return new NotInDbSet<T>(input);
    }
}

public class NotInDbSet<T> : IQueryable<T>, IAsyncEnumerable<T>, IEnumerable<T>, IEnumerable
{
    private readonly List<T> _innerCollection;
    public NotInDbSet(IEnumerable<T> innerCollection)
    {
        _innerCollection = innerCollection.ToList();
    }


    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken())
    {
        return new AsyncEnumerator(GetEnumerator());
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _innerCollection.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public class AsyncEnumerator : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _enumerator;
        public AsyncEnumerator(IEnumerator<T> enumerator)
        {
            _enumerator = enumerator;
        }

        public ValueTask DisposeAsync()
        {
            return new ValueTask();
        }

        public ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(_enumerator.MoveNext());
        }

        public T Current => _enumerator.Current;
    }

    public Type ElementType => typeof(T);
    public Expression Expression => Expression.Empty();
    public IQueryProvider Provider => new EnumerableQuery<T>(Expression);
}
