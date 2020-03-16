using System;
using System.Collections.Generic;
using System.Text;
using DataManager.Models;
using DataManager.Utilities;

namespace DataManager.Processors
{
    /// <summary>
    /// Interacts with "network" table/model stored data in DB.
    /// </summary>
    public class NetworkProcessor
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public NetworkProcessor(ISqlDataAccess sqlDataAccess) =>
            _sqlDataAccess = sqlDataAccess;

        public NetModel Load(int id)
        {
            const string sql = "SELECT * FROM [dbo].[Connection] WHERE Id=@Id";
            var batch = _sqlDataAccess.LoadDataWith<NetModel>(sql, new { Id = id });

            // Element not found
            if (batch.Count == 0)
                throw new Exception($"{nameof(NetModel)} with {nameof(id)}: {id} not found");

            return batch[0];
        }

        public void Save(NetModel netModel)
        {
            throw new NotImplementedException();
        }

        public void Delete(NetModel netModel)
        {
            throw new NotImplementedException();
        }

        public void Update(NetModel netModel)
        {
            throw new NotImplementedException();
        }
    }
}
