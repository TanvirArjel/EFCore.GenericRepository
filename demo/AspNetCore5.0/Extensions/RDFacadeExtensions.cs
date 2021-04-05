using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCore5._0.Extensions
{
    public static class RDFacadeExtensions
    {
        public static RelationalDataReader ExecuteSqlQuery(this DbContext dbContext, string sql, params object[] parameters)
        {
            IConcurrencyDetector concurrencyDetector = dbContext.Database.GetService<IConcurrencyDetector>();

            using (concurrencyDetector.EnterCriticalSection())
            {
                RawSqlCommand rawSqlCommand = dbContext.Database
                    .GetService<IRawSqlCommandBuilder>()
                    .Build(sql, parameters);

                RelationalCommandParameterObject paramObject = new(
                    dbContext.Database.GetService<IRelationalConnection>(),
                    rawSqlCommand.ParameterValues,
                    null, dbContext, null);

                return rawSqlCommand
                    .RelationalCommand
                    .ExecuteReader(paramObject);
            }
        }

        public static async Task<RelationalDataReader> ExecuteSqlCommandAsync(this DatabaseFacade databaseFacade,
                                                             string sql,
                                                             CancellationToken cancellationToken = default(CancellationToken),
                                                             params object[] parameters)
        {

            IConcurrencyDetector concurrencyDetector = databaseFacade.GetService<IConcurrencyDetector>();

            using (concurrencyDetector.EnterCriticalSection())
            {
                RawSqlCommand rawSqlCommand = databaseFacade
                    .GetService<IRawSqlCommandBuilder>()
                    .Build(sql, parameters);

                RelationalCommandParameterObject paramObject = new(databaseFacade.GetService<IRelationalConnection>(), rawSqlCommand.ParameterValues, null, null, null);

                return await rawSqlCommand
                    .RelationalCommand
                    .ExecuteReaderAsync(paramObject, cancellationToken);
            }
        }
    }
}
