using System;
using System.Collections.Generic;
using System.Text;
using DataManager.Models;
using DataManager.Utilities;

namespace DataManager.Processors
{
    public class BatchProcessor
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public BatchProcessor(ISqlDataAccess sqlDataAccess) =>
            _sqlDataAccess = sqlDataAccess;

        public BatchModel Load(int id)
        {
            const string sql = "SELECT * FROM [dbo].[Batch] WHERE Id=@Id";
            var batch = _sqlDataAccess.LoadDataWith<BatchModel>(sql, new {Id = id});

            // Element not found
            if (batch.Count == 0)
                throw new Exception($"{nameof(BatchModel)} with {nameof(id)}: {id} not found");

            return batch[0];
        }

        public void Save(BatchModel batchModel)
        {
            throw new NotImplementedException();
        }

        public void Delete(BatchModel batchModel)
        {
            throw new NotImplementedException();
        }

        public void Update(BatchModel batchModel)
        {
            throw new NotImplementedException();
        }
    }
}
