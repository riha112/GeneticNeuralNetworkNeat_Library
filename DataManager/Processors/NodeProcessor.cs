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
    public class NodeProcessor : INodeProcessor
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public NodeProcessor(ISqlDataAccess sqlDataAccess) =>
            _sqlDataAccess = sqlDataAccess;

        public NodeModel Load(int id)
        {
            const string sql = "SELECT * FROM [dbo].[Node] WHERE [Id]=@Id";
            var nodes = _sqlDataAccess.LoadDataWith<NodeModel>(sql, new { Id = id });

            // Element not found
            if (nodes.Count == 0)
                throw new Exception($"{nameof(NodeModel)} with {nameof(id)}: {id} not found");

            return nodes[0];
        }

        public List<NodeModel> LoadLinked(int networkId)
        {
            const string sql = "SELECT * FROM [dbo].[Node] WHERE [NetworkId]=@NetworkId";
            return _sqlDataAccess.LoadDataWith<NodeModel>(sql, new {NetworkId = networkId});
        }

        public void Save(ref NodeModel nodeModel)
        {
            const string sql = "INSERT INTO [dbo].[NODE] (NetworkId, InnovationId) Values (@NetworkId, @InnovationId)";
            try
            {
                var output = _sqlDataAccess.SaveData<NodeModel>(nodeModel, sql);
                nodeModel.Id = output;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Delete(int nodeId)
        {
            const string sql = "DELETE FROM [dbo].[Node] WHERE [Id]=@Id";
            _sqlDataAccess.DeleteData(sql, new { Id = nodeId });
        }

        public void Update(NodeModel nodeModel)
        {
            const string sql =@"UPDATE [dbo].[Node] WHERE 
                                [Id]=@Id, 
                                [NetworkId]=@NetworkId, 
                                [InnovationId]=@InnovationId";
            _sqlDataAccess.UpdateData(sql, nodeModel);
        }
    }
}
