using Dapper;
using NotificationSystem.DataAccessLayer.Interfaces;
using NotificationSystem.Models.Source;
using System.Data;

namespace NotificationSystem.DataAccessLayer.Implementation
{
    public class SourceRepository : RepositoryBase, ISourceRepository
    {
        private const string SOURCE_GET_BY_ID = "Source_Get_ById";
        public SourceRepository(IDbTransaction transaction, IDbConnection connection) : base(transaction, connection)
        {
        }

        public async Task<SourceDto> GetSourceById(int sourceId)
        {
            var parameters = new DynamicParameters(new
            {
                SourceId = sourceId
            });

            var sources = await Connection.QueryAsync<SourceDto>(
                sql : SOURCE_GET_BY_ID,
                param: parameters,
                commandType: CommandType.StoredProcedure,
                transaction: Transaction
                );

            return sources.FirstOrDefault();
        }
    }
}
