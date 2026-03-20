using Microsoft.Data.SqlClient;
using SoccerPro.Domain.Entities;
using SoccerPro.Domain.Entities.Views;
using SoccerPro.Domain.IRepository;
using System.Data;

namespace SoccerPro.Infrastructure.Repository;

public class CoachRepository : ICoachRepository
{
    private readonly IDbConnection _connection;

    public CoachRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<int?> AddCoachAsync(Coache coach)
    {
        using var connection = new SqlConnection(_connection.ConnectionString);
        using var command = new SqlCommand("SP_AddCoach", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        // Person parameters
        command.Parameters.AddWithValue("@KFUPMId", coach.Person.KFUPMId);
        command.Parameters.AddWithValue("@FirstName", coach.Person.FirstName);
        command.Parameters.AddWithValue("@SecondName", coach.Person.SecondName ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@ThirdName", coach.Person.ThirdName ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@LastName", coach.Person.LastName);
        command.Parameters.AddWithValue("@DateOfBirth", coach.Person.DateOfBirth);
        command.Parameters.AddWithValue("@NationalityId", coach.Person.NationalityId ?? (object)DBNull.Value);

        // Table-valued parameter
        var contactInfoTable = new DataTable();
        contactInfoTable.Columns.Add("ContactType", typeof(int));
        contactInfoTable.Columns.Add("Value", typeof(string));

        foreach (var contactInfo in coach.Person.PersonalContactInfos)
        {
            contactInfoTable.Rows.Add((int)contactInfo.ContactType, contactInfo.Value);
        }

        var contactInfoParam = command.Parameters.AddWithValue("@ContactInfos", contactInfoTable);
        contactInfoParam.SqlDbType = SqlDbType.Structured;
        contactInfoParam.TypeName = "dbo.ContactInfoType";

        // ✅ Output parameter for PersonId
        var personIdParam = new SqlParameter("@PersonId", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };
        command.Parameters.Add(personIdParam);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();

        return personIdParam.Value == DBNull.Value ? null : (int?)personIdParam.Value;
    }

    public async Task<(List<CoachView> coaches, int totalCount)> GetCoachesAsync(
    string? kfupmId = null,
    string? firstName = null,
    string? secondName = null,
    string? thirdName = null,
    string? lastName = null,
    DateTime? dateOfBirth = null,
    int? nationalityId = null,
    string? teamName = null,
    bool? isActive = null,
    int pageNumber = 1,
    int pageSize = 10)
    {
        using var connection = new SqlConnection(_connection.ConnectionString);
        using var command = new SqlCommand("SP_SearchCoaches", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@KFUPMId", kfupmId ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@FirstName", firstName ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@SecondName", secondName ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@ThirdName", thirdName ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@LastName", lastName ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@DateOfBirth", dateOfBirth ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@NationalityId", nationalityId ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@TeamName", teamName ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@IsActive", isActive ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@PageNumber", pageNumber);
        command.Parameters.AddWithValue("@PageSize", pageSize);



        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();

        var coaches = new List<CoachView>();

        while (await reader.ReadAsync())
        {
            var coach = new CoachView
            {
                CoachId = reader.GetInt32(reader.GetOrdinal("CoachId")),
                PersonId = reader.GetInt32(reader.GetOrdinal("PersonId")),
                KFUPMId = reader.GetString(reader.GetOrdinal("KFUPMId")),
                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                SecondName = reader.IsDBNull(reader.GetOrdinal("SecondName")) ? null : reader.GetString(reader.GetOrdinal("SecondName")),
                ThirdName = reader.IsDBNull(reader.GetOrdinal("ThirdName")) ? null : reader.GetString(reader.GetOrdinal("ThirdName")),
                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                NationalityId = reader.IsDBNull(reader.GetOrdinal("NationalityId")) ? null : reader.GetInt32(reader.GetOrdinal("NationalityId")),
                TeamId = reader.IsDBNull(reader.GetOrdinal("TeamId")) ? null : reader.GetInt32(reader.GetOrdinal("TeamId")),
                TeamName = reader.IsDBNull(reader.GetOrdinal("TeamName")) ? null : reader.GetString(reader.GetOrdinal("TeamName")),
                JoinedAt = reader.IsDBNull(reader.GetOrdinal("JoinedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("JoinedAt")),
                LeftAt = reader.IsDBNull(reader.GetOrdinal("LeftAt")) ? null : reader.GetDateTime(reader.GetOrdinal("LeftAt"))
            };

            coaches.Add(coach);
        }


        return (coaches, coaches.Count);
    }


    public async Task<bool> AssignCoachToTeamAsync(CoachTeam coachTeam)
    {
        using var connection = new SqlConnection(_connection.ConnectionString);
        using var command = new SqlCommand("SP_AssignCoachToTeam", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@CoachId", coachTeam.CoachId);
        command.Parameters.AddWithValue("@TeamId", coachTeam.TeamId);
        command.Parameters.AddWithValue("@JoinedAt", DateTime.Now);

        await connection.OpenAsync();
        var result = await command.ExecuteNonQueryAsync();
        return result > 0;
    }



    public async Task<bool> IsCoachAlreadyAssignedAsync(int coachId, int tournamentId)
    {
        using var connection = new SqlConnection(_connection.ConnectionString);
        using var command = new SqlCommand("CheckCoachAlreadyAssigned", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@CoachId", coachId);
        command.Parameters.AddWithValue("@TournamentId", tournamentId);

        await connection.OpenAsync();

        var result = await command.ExecuteScalarAsync();

        return Convert.ToInt32(result) == 1;
    }



}