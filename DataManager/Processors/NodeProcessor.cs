using System;
using System.Collections.Generic;
using System.Text;
using DataManager.Models;
using DataManager.Utilities;

namespace DataManager.Processors
{
    /// <summary>
    /// Interacts with "node" table/model stored data in DB.
    /// </summary>
    public class NodeProcessor
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public NodeProcessor(ISqlDataAccess sqlDataAccess) =>
            _sqlDataAccess = sqlDataAccess;

        public NodeModel Load(int id)
        {
            const string sql = "SELECT * FROM [dbo].[Connection] WHERE Id=@Id";
            var batch = _sqlDataAccess.LoadDataWith<NodeModel>(sql, new { Id = id });

            // Element not found
            if (batch.Count == 0)
                throw new Exception($"{nameof(NodeModel)} with {nameof(id)}: {id} not found");

            return batch[0];
        }

        public void Save(NodeModel nodeModel)
        {
            throw new NotImplementedException();
        }

        public void Delete(NodeModel nodeModel)
        {
            throw new NotImplementedException();
        }

        public void Update(NodeModel nodeModel)
        {
            throw new NotImplementedException();
        }
    }
}
