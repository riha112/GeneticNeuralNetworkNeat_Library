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
    public class InnovationProcessor
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public InnovationProcessor(ISqlDataAccess sqlDataAccess) =>
            _sqlDataAccess = sqlDataAccess;

        public InnovationModel Load(int id)
        {
            const string sql = "SELECT * FROM [dbo].[Connection] WHERE Id=@Id";
            var batch = _sqlDataAccess.LoadDataWith<InnovationModel>(sql, new { Id = id });

            // Element not found
            if (batch.Count == 0)
                throw new Exception($"{nameof(InnovationModel)} with {nameof(id)}: {id} not found");

            return batch[0];
        }

        public void Save(InnovationModel innovationModel)
        {
            throw new NotImplementedException();
        }

        public void Delete(InnovationModel innovationModel)
        {
            throw new NotImplementedException();
        }

        public void Update(InnovationModel innovationModel)
        {
            throw new NotImplementedException();
        }
    }
}
