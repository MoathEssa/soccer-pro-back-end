using Microsoft.Data.SqlClient;
using SoccerPro.Domain.Entities;
using SoccerPro.Domain.Entities.Enums;
using SoccerPro.Domain.Entities.Views;
using SoccerPro.Domain.IRepository;
using System.Data;

namespace SoccerPro.Infrastructure.Repository;

public class ManagerRepository : IManagerRepository
{
    private readonly IDbConnection _connection;

    public ManagerRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<int?> AddManagerAsync(Manager manager)
    {
        using var connection = new SqlConnection(_connection.ConnectionString);
        using var command = new SqlCommand("SP_AddManager", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        // Basic person info
        command.Parameters.AddWithValue("@KFUPMId", manager.Person.KFUPMId);
        command.Parameters.AddWithValue("@FirstName", manager.Person.FirstName);
        command.Parameters.AddWithValue("@SecondName", (object?)manager.Person.SecondName ?? DBNull.Value);
        command.Parameters.AddWithValue("@ThirdName", (object?)manager.Person.ThirdName ?? DBNull.Value);
        command.Parameters.AddWithValue("@LastName", manager.Person.LastName);
        command.Parameters.AddWithValue("@DateOfBirth", manager.Person.DateOfBirth);
        command.Parameters.AddWithValue("@NationalityId", manager.Person.NationalityId);

        // TVP: Contact info
        var contactTable = new DataTable();
        contactTable.Columns.Add("ContactType", typeof(int));
        contactTable.Columns.Add("Value", typeof(string));

        foreach (var contact in manager.Person.PersonalContactInfos)
        {
            contactTable.Rows.Add((int)contact.ContactType, contact.Value);
        }

        command.Parameters.Add(new SqlParameter("@ContactInfos", SqlDbType.Structured)
        {
            TypeName = "ContactInfoType",
            Value = contactTable
        });

        // Output parameter for PersonId
        var personIdParam = new SqlParameter("@PersonId", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };
        command.Parameters.Add(personIdParam);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();

        return personIdParam.Value == DBNull.Value ? null : (int?)personIdParam.Value;
    }


    public async Task<ManagerView?> GetManagerByIdAsync(int managerId)
    {
        using var connection = new SqlConnection(_connection.ConnectionString);
        using var command = new SqlCommand("SP_GetManagerById", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@ManagerId", managerId);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        var managerView = new ManagerView
        {
            ManagerId = managerId,
            KFUPMId = reader.GetString(reader.GetOrdinal("KFUPMId")),
            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
            SecondName = reader.IsDBNull(reader.GetOrdinal("SecondName")) ? null : reader.GetString(reader.GetOrdinal("SecondName")),
            ThirdName = reader.IsDBNull(reader.GetOrdinal("ThirdName")) ? null : reader.GetString(reader.GetOrdinal("ThirdName")),
            LastName = reader.GetString(reader.GetOrdinal("LastName")),
            DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
            NationalityId = reader.GetInt32(reader.GetOrdinal("NationalityId")),
            TeamName = reader.IsDBNull(reader.GetOrdinal("TeamName")) ? null : reader.GetString(reader.GetOrdinal("TeamName")),
            PersonalContactInfo = []
        };

        // Read second result set (contact info)
        if (await reader.NextResultAsync())
        {
            while (await reader.ReadAsync())
            {
                managerView.PersonalContactInfo.Add(new PersonalContactInfo
                {
                    ContactType = (ContactType)reader.GetInt32(reader.GetOrdinal("ContactType")),
                    Value = reader.GetString(reader.GetOrdinal("Value"))
                });
            }
        }

        return managerView;
    }

    public async Task<(List<ManagerSearchView> managers, int totalCount)> SearchManagersAsync(
            string? kfupmId = null,
            string? firstName = null,
            string? secondName = null,
            string? thirdName = null,
            string? lastName = null,
            DateTime? dateOfBirth = null,
            int? nationalityId = null,
            string? teamName = null,
            int pageNumber = 1,
            int pageSize = 10)
    {
        using var connection = new SqlConnection(_connection.ConnectionString);
        using var command = new SqlCommand("SP_SearchManagers", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@KFUPMId", string.IsNullOrWhiteSpace(kfupmId) ? DBNull.Value : kfupmId);
        command.Parameters.AddWithValue("@FirstName", string.IsNullOrWhiteSpace(firstName) ? DBNull.Value : firstName);
        command.Parameters.AddWithValue("@SecondName", string.IsNullOrWhiteSpace(secondName) ? DBNull.Value : secondName);
        command.Parameters.AddWithValue("@ThirdName", string.IsNullOrWhiteSpace(thirdName) ? DBNull.Value : thirdName);
        command.Parameters.AddWithValue("@LastName", string.IsNullOrWhiteSpace(lastName) ? DBNull.Value : lastName);
        command.Parameters.AddWithValue("@DateOfBirth", dateOfBirth.HasValue ? dateOfBirth.Value : DBNull.Value);
        command.Parameters.AddWithValue("@NationalityId", nationalityId.HasValue ? nationalityId.Value : DBNull.Value);
        command.Parameters.AddWithValue("@TeamName", string.IsNullOrWhiteSpace(teamName) ? DBNull.Value : teamName);
        command.Parameters.AddWithValue("@PageNumber", pageNumber);
        command.Parameters.AddWithValue("@PageSize", pageSize);


        var outputParam = new SqlParameter("@TotalCount", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };
        command.Parameters.Add(outputParam);

        var managers = new List<ManagerSearchView>();

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var manager = new ManagerSearchView
            {
                ManagerId = reader.GetInt32(reader.GetOrdinal("ManagerId")),
                KFUPMId = reader.GetString(reader.GetOrdinal("KFUPMId")),
                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                SecondName = reader.IsDBNull(reader.GetOrdinal("SecondName")) ? null : reader.GetString(reader.GetOrdinal("SecondName")),
                ThirdName = reader.IsDBNull(reader.GetOrdinal("ThirdName")) ? null : reader.GetString(reader.GetOrdinal("ThirdName")),
                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                NationalityId = reader.GetInt32(reader.GetOrdinal("NationalityId")),
                TeamName = reader.IsDBNull(reader.GetOrdinal("TeamName")) ? null : reader.GetString(reader.GetOrdinal("TeamName")),
            };

            managers.Add(manager);
        }

        return (managers, managers.Count);
    }

}