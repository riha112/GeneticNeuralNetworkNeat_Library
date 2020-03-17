using System;
using System.Collections.Generic;
using System.Text;
using DataManager.Models;
using DataManager.Utilities;

namespace DataManager.Processors
{
    /// <summary>
    /// Interacts with "innovation" table/model stored data in DB.
    /// </summary>
    public class InnovationProcessor : IInnovationProcessor
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public InnovationProcessor(ISqlDataAccess sqlDataAccess) =>
            _sqlDataAccess = sqlDataAccess;

        public InnovationModel Load(int id)
        {
            const string sql = "SELECT * FROM [dbo].[Innovation] WHERE [Id]=@Id";
            var innovations = _sqlDataAccess.LoadDataWith<InnovationModel>(sql, new { Id = id });

            // Element not found
            if (innovations.Count == 0)
                throw new Exception($"{nameof(InnovationModel)} with {nameof(id)}: {id} not found");

            return innovations[0];
        }

        public InnovationModel Find(InnovationType type, int from, int to, int batchId)
        {
            // Calling Stored procedure: GetInnovation
            const string sql = "[dbo].[GetInnovation]";
            var innovations = _sqlDataAccess.LoadDataWith<InnovationModel>(sql, new
            {
                Type = type,
                From = from,
                To = to,
                Bactch = batchId
            });

            // Element not found
            if (innovations.Count == 0)
                throw new Exception($"{nameof(InnovationModel)} not found");

            return innovations[0];
        }

        public List<InnovationModel> LoadLinked(int batchId)
        {
            const string sql = "SELECT * FROM [dbo].[Innovation] WHERE [BatchId]=@BatchId";
            return _sqlDataAccess.LoadDataWith<InnovationModel>(sql, new { BatchId = batchId });
        }

        public void Save(ref InnovationModel innovationModel)
        {
            const string sql = "INSERT INTO [dbo].[Innovation] (NodeFromId, NodeToId, Type, BatchId) Values (@NodeFromId, @NodeToId, @Type, @BatchId)";
            try
            {
                var output = _sqlDataAccess.SaveData<InnovationModel>(innovationModel, sql);
                innovationModel.Id = output;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Delete(int innovationId)
        {
            const string sql = "DELETE FROM [dbo].[Innovation] WHERE [Id]=@Id";
            _sqlDataAccess.DeleteData(sql, new { Id = innovationId });
        }

        public void Update(InnovationModel innovationModel)
        {
            const string sql = @"UPDATE [dbo].[Innovation] WHERE 
                                [NodeFromId]=@NodeFromId, 
                                [NodeToId]=@NodeToId, 
                                [Type]=@Type,
                                [BatchId]=@BatchId";
            _sqlDataAccess.UpdateData(sql, innovationModel);
        }
    }
}
