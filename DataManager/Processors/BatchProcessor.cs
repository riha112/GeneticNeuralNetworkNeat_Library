using System;
using System.Collections.Generic;
using System.Text;
using DataManager.Models;
using DataManager.Utilities;

namespace DataManager.Processors
{
    /// <summary>
    /// Interacts with "batch" table/model stored data in DB.
    /// </summary>
    public class BatchProcessor : IBatchProcessor
    {
        private readonly ISqlDataAccess _sqlDataAccess;
        private readonly INetworkProcessor _networkProcessor;
        private readonly IInnovationProcessor _innovationProcessor;

        public BatchProcessor(ISqlDataAccess sqlDataAccess, INetworkProcessor networkProcessor, IInnovationProcessor innovationProcessor) =>
            (_sqlDataAccess, _networkProcessor, _innovationProcessor) = (sqlDataAccess, networkProcessor, innovationProcessor);

        public BatchModel Load(int id)
        {
            const string sql = "SELECT * FROM [dbo].[Batch] WHERE [Id]=@Id";
            var batches = _sqlDataAccess.LoadDataWith<BatchModel>(sql, new {Id = id});

            // Element not found
            if (batches.Count == 0)
                throw new Exception($"{nameof(BatchModel)} with {nameof(id)}: {id} not found");

            var batch = batches[0];
            batch.Networks = _networkProcessor.LoadLinked(batch.Id);
            batch.Innovations = _innovationProcessor.LoadLinked(batch.Id);

            return batches[0];
        }

        public void Save(ref BatchModel batchModel)
        {
            const string sql = @"INSERT INTO [dbo].[Batch] (Name, Description, Generation) 
                                 VALUES (@Name, @Description, @Generation);
                                 SELECT CAST(SCOPE_IDENTITY() as int)";
            try
            {
                var output = _sqlDataAccess.SaveData<BatchModel>(batchModel, sql);
                batchModel.Id = output;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Delete(int batchId)
        {
            const string sql = "DELETE FROM [dbo].[Batch] WHERE [Id]=@Id";
            _sqlDataAccess.DeleteData(sql, new { Id = batchId });
        }

        public void Update(BatchModel batchModel)
        {
            const string sql = @"UPDATE [dbo].[Batch] SET 
                                [Name]=@FromId, 
                                [Description]=@Description,
                                [Generation]=@Generation
                                WHERE [Id]=@Id";
            _sqlDataAccess.UpdateData(sql, batchModel);
        }
    }
}
