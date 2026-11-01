
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace API_with_DOCKER.Performance
{
    public sealed class SqlQueryCounterInterceptor : DbCommandInterceptor
    {
        private readonly SqlQueryCounter _counter;
        public SqlQueryCounterInterceptor(SqlQueryCounter counter)
        {
            _counter = counter;
        }

        public override ValueTask<InterceptionResult<DbDataReader>>ReaderExecutingAsync(DbCommand command,CommandEventData eventData, InterceptionResult<DbDataReader> result,CancellationToken cancellationToken = default)
        {
            _counter.Increment();

            return new ValueTask<InterceptionResult<DbDataReader>>(result);
        }
    }
}
