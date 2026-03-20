using Microsoft.Data.SqlClient;
using SoccerPro.Application.Services.IServises;
using SoccerPro.Domain.Entities;
using SoccerPro.Domain.Entities.Enums;
using System.Data;

namespace SoccerPro.Infrastructure.Repository;

public class SharedRepossitory : ISharedRepository
{
    private readonly IDbConnection _db;

    public SharedRepossitory(IDbConnection db)
    {
        _db = db;
    }

    public async Task<List<Country>> GetAllCountriesAsync()
    {
        var countries = new List<Country>();
        const string sql = @"
            SELECT 
                *
            FROM Countries;
        ";

        await using var conn = new SqlConnection(_db.ConnectionString);
        await using var cmd = new SqlCommand(sql, conn);

        await conn.OpenAsync();
        await using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);

        while (await reader.ReadAsync())
        {
            countries.Add(new Country
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name"))
            });
        }

        return countries;
    }

    public async Task<List<Department>> GetAllDepartmentsAsync()
    {
        var departments = new List<Department>();
        const string sql = @"SELECT DepartmentId, Name FROM Departments;";

        await using var conn = new SqlConnection(_db.ConnectionString);
        await using var cmd = new SqlCommand(sql, conn);

        await conn.OpenAsync();
        await using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);

        while (await reader.ReadAsync())
        {
            departments.Add(new Department
            {
                DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                Name = reader.GetString(reader.GetOrdinal("Name"))
            });
        }

        return departments;
    }

    public async Task<List<PersonalContactInfo>> GetPersonalContactInfoByPersonIdAsync(int personId)
    {
        var contactInfos = new List<PersonalContactInfo>();
        const string sql = @"
            SELECT 
                PersonalContactInfoId,
                PersonId,
                ContactType,
                Value
            FROM PersonalContactsInfos
            WHERE PersonId = @PersonId;";

        await using var conn = new SqlConnection(_db.ConnectionString);
        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@PersonId", personId);

        await conn.OpenAsync();
        await using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);

        while (await reader.ReadAsync())
        {
            contactInfos.Add(new PersonalContactInfo
            {
                PersonalContactInfoId = reader.GetInt32(reader.GetOrdinal("PersonalContactInfoId")),
                PersonId = reader.GetInt32(reader.GetOrdinal("PersonId")),
                ContactType = (ContactType)reader.GetInt32(reader.GetOrdinal("ContactType")),
                Value = reader.GetString(reader.GetOrdinal("Value"))
            });
        }

        return contactInfos;
    }
}
