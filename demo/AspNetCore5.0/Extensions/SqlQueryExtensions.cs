using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace AspNetCore5._0.Extensions
{
    public static class SqlQueryExtensions
    {
        public static List<T> GetFromQuery<T>(this DbContext dbContext, string sql, params object[] parameters)
        {
            using DbDataReader dr = dbContext.ExecuteSqlQuery(sql, parameters).DbDataReader;

            List<T> list = new List<T>();
            T t = default;
            while (dr.Read())
            {
                if (!(typeof(T).IsPrimitive || typeof(T).Equals(typeof(string))))
                {
                    PropertyInfo[] props = typeof(T).GetProperties();
                    IEnumerable<string> actualNames = dr.GetColumnSchema().Select(o => o.ColumnName);
                    for (int i = 0; i < props.Length; ++i)
                    {
                        PropertyInfo pi = props[i];

                        if (!pi.CanWrite) continue;

                        ColumnAttribute ca = pi.GetCustomAttribute(typeof(ColumnAttribute)) as ColumnAttribute;
                        string name = ca?.Name ?? pi.Name;

                        if (pi == null) continue;

                        if (!actualNames.Contains(name))
                        {
                            continue;
                        }
                        object value = dr[name];
                        Type pt = pi.DeclaringType;
                        bool nullable = pt.GetTypeInfo().IsGenericType && pt.GetGenericTypeDefinition() == typeof(Nullable<>);
                        if (value == DBNull.Value)
                        {
                            value = null;
                        }
                        if (value == null && pt.GetTypeInfo().IsValueType && !nullable)
                        {
                            value = Activator.CreateInstance(pt);
                        }
                        pi.SetValue(t, value);
                    }//for i

                    list.Add(t);
                }
                else
                {
                    t = (T)Convert.ChangeType(dr[0], typeof(T));
                    list.Add(t);
                }
            }//while
            return list;
            //using dr
        }

        public static List<T> ExecSQL<T>(this DbContext context, string query)
        {
            using (context)
            {
                using DbCommand command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                context.Database.OpenConnection();

                using DbDataReader result = command.ExecuteReader();

                List<T> list = new List<T>();
                T obj = default;
                while (result.Read())
                {
                    if (!(typeof(T).IsPrimitive || typeof(T).Equals(typeof(string))))
                    {
                        obj = Activator.CreateInstance<T>();
                        foreach (PropertyInfo prop in obj.GetType().GetProperties())
                        {
                            if (!object.Equals(result[prop.Name], DBNull.Value))
                            {
                                prop.SetValue(obj, result[prop.Name], null);
                            }
                        }
                        list.Add(obj);
                    }
                    else
                    {
                        obj = (T)Convert.ChangeType(result[0], typeof(T));
                        list.Add(obj);
                    }

                }
                return list;
            }
        }

        public static List<T> ExecSQL<T>(this DbContext context, string query, object parameter)
        {
            using (context)
            {
                SqlParameter paramter = new SqlParameter();
                using DbCommand command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = query;
                command.Parameters.Add(parameter);
                command.CommandType = CommandType.Text;
                context.Database.OpenConnection();

                using DbDataReader result = command.ExecuteReader();

                List<T> list = new List<T>();
                T obj = default;
                while (result.Read())
                {
                    if (!(typeof(T).IsPrimitive || typeof(T).Equals(typeof(string))))
                    {
                        obj = Activator.CreateInstance<T>();
                        foreach (PropertyInfo prop in obj.GetType().GetProperties())
                        {
                            if (!object.Equals(result[prop.Name], DBNull.Value))
                            {
                                prop.SetValue(obj, result[prop.Name], null);
                            }
                        }
                        list.Add(obj);
                    }
                    else
                    {
                        obj = (T)Convert.ChangeType(result[0], typeof(T));
                        list.Add(obj);
                    }

                }
                return list;
            }
        }

    }
}
