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
    public class ConnectionProcessor : IConnectionProcessor
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public ConnectionProcessor(ISqlDataAccess sqlDataAccess) =>
            _sqlDataAccess = sqlDataAccess;

        public ConnectionModel Load(int id)
        {
            const string sql = "SELECT * FROM [dbo].[Connection] WHERE [Id]=@Id ORDER BY [InnovationId]";
            var connections = _sqlDataAccess.LoadDataWith<ConnectionModel>(sql, new { Id = id });

            // Element not found
            if (connections.Count == 0)
                throw new Exception($"{nameof(ConnectionModel)} with {nameof(id)}: {id} not found");

            return connections[0];
        }

        public List<ConnectionModel> LoadLinked(int networkId)
        {
            const string sql = "SELECT * FROM [dbo].[Connection] WHERE [NetworkId]=@NetworkId ORDER BY [InnovationId]";
            return _sqlDataAccess.LoadDataWith<ConnectionModel>(sql, new { NetworkId = networkId });
        }

        public void Save(ref ConnectionModel connectionModel)
        {
            const string sql = @"INSERT INTO [dbo].[Connection] (FromId, ToId, Weight, InnovationId, NetworkId) 
                                 Values (@FromId, @ToId, @Weight, @InnovationId, @NetworkId);
                                 SELECT CAST(SCOPE_IDENTITY() as int)";
            try
            {
                var output = _sqlDataAccess.SaveData<ConnectionModel>(connectionModel, sql);
                connectionModel.Id = output;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Delete(int connectionId)
        {
            const string sql = "DELETE FROM [dbo].[Connection] WHERE [Id]=@Id";
            _sqlDataAccess.DeleteData(sql, new { Id = connectionId });
        }

        public void Update(ConnectionModel connectionModel)
        {
            const string sql = @"UPDATE [dbo].[Connection] SET 
                                [FromId]=@FromId, 
                                [ToId]=@ToId,
                                [Weight]=@Weight, 
                                [InnovationId]=@InnovationId, 
                                [NetworkId]=@NetworkId, 
                                [Enabled]=@Enabled
                                WHERE [Id]=@Id";
            _sqlDataAccess.UpdateData(sql, connectionModel);
        }
    }
}
