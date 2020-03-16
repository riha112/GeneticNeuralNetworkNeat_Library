using System;
using System.Collections.Generic;
using System.Text;
using DataManager.Models;
using DataManager.Utilities;

namespace DataManager.Processors
{   
    /// <summary>
    /// Interacts with "connection" table/model stored data in DB.
    /// </summary>
    public class ConnectionProcessor
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public ConnectionProcessor(ISqlDataAccess sqlDataAccess) =>
            _sqlDataAccess = sqlDataAccess;

        public ConnectionModel Load(int id)
        {
            const string sql = "SELECT * FROM [dbo].[Connection] WHERE Id=@Id";
            var batch = _sqlDataAccess.LoadDataWith<ConnectionModel>(sql, new { Id = id });

            // Element not found
            if (batch.Count == 0)
                throw new Exception($"{nameof(ConnectionModel)} with {nameof(id)}: {id} not found");

            return batch[0];
        }

        public void Save(ConnectionModel connectionModel)
        {
            throw new NotImplementedException();
        }

        public void Delete(ConnectionModel connectionModel)
        {
            throw new NotImplementedException();
        }

        public void Update(ConnectionModel connectionModel)
        {
            throw new NotImplementedException();
        }
    }
}
