using System.Collections.Generic;

namespace DataManager.Utilities
{
    public interface ISqlDataAccess
    {
        List<T> LoadData<T>(string sql);
        List<T> LoadDataWith<T>(string sql, object data);
        void SaveData<T>(T savable, string sql);
        void UpdateData<T>(T updatable, string sql);
        void DeleteData<T>(T deletable, string sql);
    }
}