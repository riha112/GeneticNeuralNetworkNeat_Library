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
    public class NetworkProcessor : INetworkProcessor
    {
        private readonly ISqlDataAccess _sqlDataAccess;
        private readonly INodeProcessor _nodeProcessor;
        private readonly IConnectionProcessor _connectionProcessor;

        public NetworkProcessor(ISqlDataAccess sqlDataAccess, INodeProcessor nodeProcessor, IConnectionProcessor connectionProcessor) =>
            (_sqlDataAccess, _nodeProcessor, _connectionProcessor) = (sqlDataAccess, nodeProcessor, connectionProcessor);

        public NetModel Load(int id)
        {
            const string sql = "SELECT * FROM [dbo].[NET] WHERE [Id]=@Id AND [Enabled]=1";
            var networks = _sqlDataAccess.LoadDataWith<NetModel>(sql, new { Id = id });

            // Element not found
            if (networks.Count == 0)
                throw new Exception($"{nameof(NetModel)} with {nameof(id)}: {id} not found");

            var network = networks[0];
            network.Connections = _connectionProcessor.LoadLinked(network.Id);
            network.Nodes = _nodeProcessor.LoadLinked(network.Id);

            return network;
        }

        public List<NetModel> LoadLinked(int batchId)
        {
            //const string sql = @"SELECT net.*, conn.*, node.*
            //            FROM [dbo].[NET] AS net
            //            LEFT JOIN [dbo].[Connection] AS conn ON conn.NetworkId = net.Id
            //            LEFT JOIN [dbo].[NODE] AS node ON node.NetworkId = net.Id
            //            WHERE net.BatchId = @BatchId";

            //var networks = _sqlDataAccess.MapOf3<NetModel, ConnectionModel, NodeModel, NetModel>( 
            //    sql, new { BatchId = batchId}, (net, conn, node) =>
            //    {
            //        net.Connections.Add(conn);
            //        net.Nodes.Add(node);
            //        return net;
            //    },  "NetworkId");

            const string sql = @"SELECT * FROM [dbo].[NET] WHERE [BatchId]=@BatchId AND [Enabled]=1";
            var networks = _sqlDataAccess.LoadDataWith<NetModel>(sql, new {BatchId = batchId});
            foreach (var network in networks)
            {
                network.Connections = _connectionProcessor.LoadLinked(network.Id);
                network.Nodes = _nodeProcessor.LoadLinked(network.Id);
            }

            return networks;
        }

        public void Save(ref NetModel netModel)
        {
            const string sql = @"INSERT INTO [dbo].[NET] (BirthGeneration, BatchId) 
                                 Values (@BirthGeneration, @BatchId);
                                 SELECT CAST(SCOPE_IDENTITY() as int)";
            try
            {
                var output = _sqlDataAccess.SaveData<NetModel>(netModel, sql);
                netModel.Id = output;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Delete(int netId)
        {
            const string sql = "DELETE FROM [dbo].[NET] WHERE [Id]=@Id";
            _sqlDataAccess.DeleteData(sql, new { Id = netId });
        }

        public void Update(NetModel netModel)
        {
            const string sql = @"UPDATE [dbo].[NET] SET
                                [FitnessScore]=@NetworkId, 
                                [BirthGeneration]=@InnovationId,
                                [BatchId]=@BatchId 
                                WHERE [Id]=@Id";
            _sqlDataAccess.UpdateData(sql, netModel);
        }
    }
}
