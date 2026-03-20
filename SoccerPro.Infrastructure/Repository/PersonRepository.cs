using Microsoft.Data.SqlClient;
using SoccerPro.Domain.Entities;
using System.Data;

namespace SoccerPro.Infrastructure.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly IDbConnection _connection;

        public PersonRepository(IDbConnection connection) => _connection = connection;

        public Task<int> AddPersonAsync(Person person)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CheckIsPersonExistAsync(string KFUPMId)
        {
            using var connection = new SqlConnection(_connection.ConnectionString);
            using var command = new SqlCommand("SP_IsPersonExistByKFUPMId", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@KFUPMId", KFUPMId);

            var outputParam = new SqlParameter("@IsExist", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(outputParam);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();

            return (bool)outputParam.Value;
        }
    }
}
