using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Data.Common;
using System.Threading.Tasks;

namespace King.EntityFrameworkCore
{
    public static class DbContextExtend
    {
        public async static Task<List<T>> GetList<T>(this DbContext dbContent, string sqlStr, params object[] sqlParames) where T : new()
        {
            List<T> ret = new List<T>();
            Type t = typeof(T);
            var connection = dbContent.Database.GetDbConnection();
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = sqlStr;
                command.Parameters.AddRange(sqlParames);
                using (var reader = command.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {
                        ret.Add(ReaderToModel<T>(reader));
                    }
                }
            }
            return ret;
        }

        public async static Task<T> GetModel<T>(this DbContext dbContent, string sqlStr, params object[] sqlParames) where T : new()
        {
            var connection = dbContent.Database.GetDbConnection();

            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = sqlStr;
                command.Parameters.AddRange(sqlParames);
                using (var reader = command.ExecuteReader())
                {
                    if (await reader.ReadAsync())
                    {
                        return ReaderToModel<T>(reader);
                    }
                    else
                    {
                        return default(T);
                    }
                }
            }
        }

        private static T ReaderToModel<T>(DbDataReader reader) where T : new()
        {
            if (reader.FieldCount == 1)
            {
                return reader.GetFieldValue<T>(0);
            }
            else
            {
                T tObj = new T();
                Type tobjType = tObj.GetType();
                PropertyInfo[] propertyInfos = tobjType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var fieldName = reader.GetName(i).ToLower();

                    if (reader[i] != DBNull.Value)
                    {
                        var propertyInfo = propertyInfos.First(p => p.Name.ToLower() == fieldName);
                        if (propertyInfo == null)
                            continue;
                        object fiedVal = reader[i];
                        fiedVal= Convert.ChangeType(fiedVal, propertyInfo.PropertyType);
                        propertyInfo.SetValue(tObj, fiedVal, null);
                    }
                }
                return tObj;
            }
        }
    }
}
