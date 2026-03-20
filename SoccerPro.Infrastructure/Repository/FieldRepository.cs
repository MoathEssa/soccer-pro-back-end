using Microsoft.Data.SqlClient;
using SoccerPro.Domain.Entities;
using SoccerPro.Domain.Entities.Enums;
using SoccerPro.Domain.IRepository;
using System.Data;

namespace SoccerPro.Infrastructure.Repository;

public class FieldRepository : IFieldRepository
{
    private readonly IDbConnection _connection;

    public FieldRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<List<Field>> GetAllFieldsAsync()
    {
        using var connection = new SqlConnection(_connection.ConnectionString);
        using var command = new SqlCommand("SP_GetAllFields", connection);


        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();

        var fields = new List<Field>();
        while (await reader.ReadAsync())
        {
            fields.Add(new Field
            {
                FieldId = reader.GetInt32(reader.GetOrdinal("FieldId")),
                Number = reader.GetString(reader.GetOrdinal("Number")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Description = reader.GetString(reader.GetOrdinal("Description")),
                AudienceCapacity = reader.GetInt32(reader.GetOrdinal("AudienceCapacity")),
                Status = (FieldStatus)reader.GetInt32(reader.GetOrdinal("Status"))
            });
        }

        return fields;
    }

}