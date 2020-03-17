using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using Dapper;

namespace DataManager.Utilities
{
    public class SqlDataAccess : ISqlDataAccess
    {
        // TODO: Replace with DI supported expression
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["GNNDB"].ConnectionString;

        public List<T> LoadData<T>(string sql)
        {
            using IDbConnection cnn = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
            var output = cnn.Query<T>(sql, new DynamicParameters());
            return output.ToList();
        }

        public List<T> LoadDataWith<T>(string sql, object parameters)
        {
            using IDbConnection cnn = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
            var output = cnn.Query<T>(sql, parameters);
            return output.ToList();
        }

        public int SaveData<T>(T savable, string sql)
        {
            using IDbConnection cnn = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
            var output = cnn.Execute(sql, savable);
            return output;
        }

        public void UpdateData(string sql, object parameters)
        {
            using IDbConnection cnn = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
            cnn.Execute(sql, parameters);
        }

        public void DeleteData(string sql, object parameters)
        {
            using IDbConnection cnn = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
            cnn.Execute(sql, parameters);
        }

        public List<TO> MapOf3<T1, T2, T3, TO>(string sql, object parameters, Func<T1, T2, T3, TO> map, string splitOn)
        {
            using IDbConnection cnn = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
            var output = cnn.Query<T1, T2, T3, TO>(sql, map, parameters, splitOn: splitOn);
            return output.ToList();
        }

    }
}
