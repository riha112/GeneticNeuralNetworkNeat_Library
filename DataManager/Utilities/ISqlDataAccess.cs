using System;
using System.Collections.Generic;
using System.Data;

namespace DataManager.Utilities
{
    public interface ISqlDataAccess
    {
        List<T> LoadData<T>(string sql);
        List<T> LoadDataWith<T>(string sql, object parameters, CommandType? type = null);

        int SaveData<T>(T savable, string sql);
        void UpdateData(string sql, object parameters);
        void DeleteData(string sql, object parameters);
        List<TO> MapOf3<T1, T2, T3, TO>(string sql, object parameters, Func<T1, T2, T3, TO> map, string splitOn);
    }
}