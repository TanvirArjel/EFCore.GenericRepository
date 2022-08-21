// <copyright file="SqlQueryExtensions.cs" company="TanvirArjel">
// Copyright (c) TanvirArjel. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TanvirArjel.EFCore.GenericRepository
{
    internal static class SqlQueryExtensions
    {
        public static async Task<List<T>> GetFromQueryAsync<T>(
            this DbContext dbContext,
            string sql,
            IEnumerable<object> parameters,
            CancellationToken cancellationToken = default)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentNullException(nameof(sql));
            }

            using DbCommand command = dbContext.Database.GetDbConnection().CreateCommand();
            command.CommandText = sql;

            if (parameters != null)
            {
                int index = 0;
                foreach (object item in parameters)
                {
                    DbParameter dbParameter = command.CreateParameter();
                    dbParameter.ParameterName = "@p" + index;
                    dbParameter.Value = item;
                    command.Parameters.Add(dbParameter);
                    index++;
                }
            }

            try
            {
                await dbContext.Database.OpenConnectionAsync(cancellationToken);

                using DbDataReader result = await command.ExecuteReaderAsync(cancellationToken);

                List<T> list = new List<T>();
                T obj = default;
                while (await result.ReadAsync(cancellationToken))
                {
                    if (!(typeof(T).IsPrimitive || typeof(T).Equals(typeof(string))))
                    {
                        obj = Activator.CreateInstance<T>();
                        foreach (PropertyInfo prop in obj.GetType().GetProperties())
                        {
                            string propertyName = prop.Name;
                            bool isColumnExistent = result.ColumnExists(propertyName);
                            if (isColumnExistent)
                            {
                                object columnValue = result[propertyName];

                                if (!Equals(columnValue, DBNull.Value))
                                {
                                    prop.SetValue(obj, columnValue, null);
                                }
                            }
                        }

                        list.Add(obj);
                    }
                    else
                    {
                        obj = (T)Convert.ChangeType(result[0], typeof(T), CultureInfo.InvariantCulture);
                        list.Add(obj);
                    }
                }

                return list;
            }
            finally
            {
                await dbContext.Database.CloseConnectionAsync();
            }
        }

        public static async Task<List<T>> GetFromQueryAsync<T>(
            this DbContext dbContext,
            string sql,
            IEnumerable<DbParameter> parameters,
            CancellationToken cancellationToken = default)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentNullException(nameof(sql));
            }

            using DbCommand command = dbContext.Database.GetDbConnection().CreateCommand();
            command.CommandText = sql;

            if (parameters != null)
            {
                foreach (DbParameter dbParameter in parameters)
                {
                    command.Parameters.Add(dbParameter);
                }
            }

            try
            {
                await dbContext.Database.OpenConnectionAsync(cancellationToken);

                using DbDataReader result = await command.ExecuteReaderAsync(cancellationToken);

                List<T> list = new List<T>();
                T obj = default;
                while (await result.ReadAsync(cancellationToken))
                {
                    if (!(typeof(T).IsPrimitive || typeof(T).Equals(typeof(string))))
                    {
                        obj = Activator.CreateInstance<T>();
                        foreach (PropertyInfo prop in obj.GetType().GetProperties())
                        {
                            string propertyName = prop.Name;
                            bool isColumnExistent = result.ColumnExists(propertyName);
                            if (isColumnExistent)
                            {
                                object columnValue = result[propertyName];

                                if (!Equals(columnValue, DBNull.Value))
                                {
                                    prop.SetValue(obj, columnValue, null);
                                }
                            }
                        }

                        list.Add(obj);
                    }
                    else
                    {
                        obj = (T)Convert.ChangeType(result[0], typeof(T), CultureInfo.InvariantCulture);
                        list.Add(obj);
                    }
                }

                return list;
            }
            finally
            {
                await dbContext.Database.CloseConnectionAsync();
            }
        }
    }
}
