using Microsoft.Data.SqlClient;
using SoccerPro.Domain.Entities;
using SoccerPro.Domain.Entities.Views;
using SoccerPro.Domain.IRepository;
using System.Data;

namespace SoccerPro.Infrastructure.Repository;

public class RefereeRepository : IRefereeRepository
{
    private readonly IDbConnection _connection;

    public RefereeRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<int?> AddRefereeAsync(
    Referee Referee)
    {
        using var connection = new SqlConnection(_connection.ConnectionString);
        using var command = new SqlCommand("SP_AddReferee", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        // Regular parameters
        command.Parameters.AddWithValue("@KFUPMId", Referee.Person.KFUPMId);
        command.Parameters.AddWithValue("@FirstName", Referee.Person.FirstName);
        command.Parameters.AddWithValue("@SecondName", (object?)Referee.Person.SecondName ?? DBNull.Value);
        command.Parameters.AddWithValue("@ThirdName", (object?)Referee.Person.ThirdName ?? DBNull.Value);
        command.Parameters.AddWithValue("@LastName", Referee.Person.LastName);
        command.Parameters.AddWithValue("@DateOfBirth", Referee.Person.DateOfBirth);
        command.Parameters.AddWithValue("@NationalityId", Referee.Person.NationalityId);

        // TVP: @ContactInfos
        var tvp = new DataTable();
        tvp.Columns.Add("ContactType", typeof(int));
        tvp.Columns.Add("Value", typeof(string));

        foreach (var info in Referee.Person.PersonalContactInfos)
        {
            tvp.Rows.Add(info.ContactType, info.Value);
        }

        var contactInfosParam = command.Parameters.AddWithValue("@ContactInfos", tvp);
        contactInfosParam.SqlDbType = SqlDbType.Structured;
        contactInfosParam.TypeName = "ContactInfoType";

        // Output: @PersonId
        var personIdParam = new SqlParameter("@PersonId", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };
        command.Parameters.Add(personIdParam);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();

        return personIdParam.Value != DBNull.Value ? (int?)personIdParam.Value : null;
    }

    public async Task<List<RefereeView>> GetAllRefereesAsync()
    {
        using var connection = new SqlConnection(_connection.ConnectionString);
        using var command = new SqlCommand("SP_GetAllReferees", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync();

        using var reader = await command.ExecuteReaderAsync();
        var referees = new List<RefereeView>();

        while (await reader.ReadAsync())
        {
            referees.Add(new RefereeView
            {
                RefereeId = reader.GetInt32(reader.GetOrdinal("RefereeId")),
                PersonId = reader.GetInt32(reader.GetOrdinal("PersonId")),
                KfupmId = reader.GetString(reader.GetOrdinal("KfupmId")),
                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                SecondName = reader.IsDBNull(reader.GetOrdinal("SecondName")) ? null : reader.GetString(reader.GetOrdinal("SecondName")),
                ThirdName = reader.IsDBNull(reader.GetOrdinal("ThirdName")) ? null : reader.GetString(reader.GetOrdinal("ThirdName")),
                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                NationalityId = reader.GetInt32(reader.GetOrdinal("NationalityId"))
            });
        }

        return referees;
    }

    public async Task<bool> AssignRefereeToTournamentAsync(int tournamentId, int refereeId)
    {
        using var connection = new SqlConnection(_connection.ConnectionString);
        using var command = new SqlCommand("SP_AssignRefereeToTournament", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@TournamentId", tournamentId);
        command.Parameters.AddWithValue("@RefereeId", refereeId);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();

        return true;
    }

    public async Task<bool> AssignTournamentRefereeToMatchAsync(int matchScheduleId, int tournamentRefereeId)
    {
        using var connection = new SqlConnection(_connection.ConnectionString);
        using var command = new SqlCommand("SP_AssignTournamentRefereeToMatch", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@MatchScheduleId", matchScheduleId);
        command.Parameters.AddWithValue("@TournamentRefereeId", tournamentRefereeId);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();

        return true;
    }

    public async Task<bool> IsRefereeExistsAsync(int refereeId)
    {
        var query = "SELECT COUNT(1) FROM Referees WHERE RefereeId = @RefereeId";

        using var command = new SqlCommand(query, _connection as SqlConnection);
        command.Parameters.AddWithValue("@RefereeId", refereeId);

        if (_connection.State != ConnectionState.Open)
            _connection.Open();

        var result = await command.ExecuteScalarAsync();
        return result != null && (int)result > 0;
    }

    public async Task<bool> IsRefereeInTournamentAsync(int refereeId, int tournamentId)
    {
        var query = @"SELECT COUNT(1) FROM TournamentsReferees 
                  WHERE RefereeId = @RefereeId AND TournamentId = @TournamentId";

        using var connection = new SqlConnection(_connection.ConnectionString);
        using var command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@RefereeId", refereeId);
        command.Parameters.AddWithValue("@TournamentId", tournamentId);

        await connection.OpenAsync();

        var result = await command.ExecuteScalarAsync();
        return result != null && Convert.ToInt32(result) > 0;
    }



    public async Task<bool> IsRefereeInSameMatchAsync(int matchScheduleId, int tournamentRefereeId)
    {
        var query = @"SELECT COUNT(1) FROM MatchesReferees 
                  WHERE 
                  TournamentRefereeId = @tournamentRefereeId AND 
                  MatchScheduleId = @matchScheduleId";

        using var connection = new SqlConnection(_connection.ConnectionString);
        using var command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@matchScheduleId", matchScheduleId);
        command.Parameters.AddWithValue("@tournamentRefereeId", tournamentRefereeId);

        await connection.OpenAsync(); // ✅ FIX: open the right connection

        var result = await command.ExecuteScalarAsync();
        return result != null && Convert.ToInt32(result) > 0;
    }


    public async Task<List<TournamentRefereeView>> GetRefereesInTournamentAsync(int tournamentId)
    {
        using var connection = new SqlConnection(_connection.ConnectionString);
        using var command = new SqlCommand("SP_GetRefereesInTournament", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@TournamentId", tournamentId);
        await connection.OpenAsync();

        var result = new List<TournamentRefereeView>();

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            result.Add(new TournamentRefereeView
            {
                TournamentRefereeId = reader.GetInt32(reader.GetOrdinal("TournamentRefereeId")),
                TournamentId = reader.GetInt32(reader.GetOrdinal("TournamentId")),
                TournamentName = reader.GetString(reader.GetOrdinal("TournamentName")),

                RefereeId = reader.GetInt32(reader.GetOrdinal("RefereeId")),
                PersonId = reader.GetInt32(reader.GetOrdinal("PersonId")),
                KFUPMId = reader.GetString(reader.GetOrdinal("KFUPMId")),
                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                SecondName = reader.IsDBNull(reader.GetOrdinal("SecondName")) ? null : reader.GetString(reader.GetOrdinal("SecondName")),
                ThirdName = reader.IsDBNull(reader.GetOrdinal("ThirdName")) ? null : reader.GetString(reader.GetOrdinal("ThirdName")),
                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                NationalityId = reader.IsDBNull(reader.GetOrdinal("NationalityId")) ? null : reader.GetInt32(reader.GetOrdinal("NationalityId"))
            });
        }

        return result;
    }


}

