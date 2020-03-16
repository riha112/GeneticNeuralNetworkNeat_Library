using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;

namespace DataManager.Utilities
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private static string ConnectionString => "";

        public List<T> LoadData<T>(string sql)
        {
            using IDbConnection cnn = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
            var output = cnn.Query<T>(sql, new DynamicParameters());
            return output.ToList();
        }

        public List<T> LoadDataWith<T>(string sql, object data)
        {
            using IDbConnection cnn = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
            var output = cnn.Query<T>(sql, data);
            return output.ToList();
        }

        public void SaveData<T>(T savable, string sql)
        {
            using IDbConnection cnn = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
            cnn.Execute(sql, savable);
        }

        public void UpdateData<T>(T updatable, string sql)
        {
            using IDbConnection cnn = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
            cnn.Execute(sql, updatable);
        }

        public void DeleteData<T>(T deletable, string sql)
        {

        }
    }
}
