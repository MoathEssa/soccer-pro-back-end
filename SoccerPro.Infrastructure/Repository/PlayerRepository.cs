using Microsoft.Data.SqlClient;
using Namespace.SoccerPro.Domain.IRepository;
using SoccerPro.Domain.Entities;
using SoccerPro.Domain.Entities.Enums;
using SoccerPro.Domain.Entities.Views;
using SoccerPro.Domain.Enums;
using System.Data;

namespace SoccerPro.Infrastructure.Repository;

public class PlayerRepository(IDbConnection connection) : IPlayerRepository
{
    private readonly IDbConnection _connection = connection;


    public async Task<int?> AddPlayerAsync(Player player)
    {
        using var command = new SqlCommand("dbo.SP_InsertPlayerWithMultipleContacts", (SqlConnection)_connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@KFUPMId", player.Person.KFUPMId);
        command.Parameters.AddWithValue("@FirstName", player.Person.FirstName);
        command.Parameters.AddWithValue("@SecondName", player.Person.SecondName ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@ThirdName", player.Person.ThirdName ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@LastName", player.Person.LastName);
        command.Parameters.AddWithValue("@DateOfBirth", player.Person.DateOfBirth);
        command.Parameters.AddWithValue("@NationalityId", player.Person.NationalityId);
        command.Parameters.AddWithValue("@PlayerType", player.PlayerType);
        command.Parameters.AddWithValue("@DepartmentId", player.DepartmentId);
        command.Parameters.AddWithValue("@PlayerStatus", player.PlayerType);

        // ContactInfos TVP
        var contactTable = new DataTable();
        contactTable.Columns.Add("ContactType", typeof(int));
        contactTable.Columns.Add("Value", typeof(string));

        foreach (var contact in player.Person.PersonalContactInfos)
        {
            contactTable.Rows.Add((int)contact.ContactType, contact.Value);
        }

        var contactInfosParam = command.Parameters.AddWithValue("@ContactInfos", contactTable);
        contactInfosParam.SqlDbType = SqlDbType.Structured;
        contactInfosParam.TypeName = "dbo.ContactInfoType";

        // OUTPUT: PersonId
        var personIdParam = new SqlParameter("@PersonId", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };
        command.Parameters.Add(personIdParam);

        if (_connection.State != ConnectionState.Open)
            await ((SqlConnection)_connection).OpenAsync();

        try
        {
            await command.ExecuteNonQueryAsync();
            return personIdParam.Value == DBNull.Value ? null : (int?)personIdParam.Value;
        }
        catch
        {
            // optionally log the exception here
            throw;
        }
    }



    public async Task<Player?> GetPlayerByIdAsync(int playerId)
    {
        using var connection = new SqlConnection(_connection.ConnectionString);
        await connection.OpenAsync();

        var query = "SELECT * FROM Players WHERE PlayerId = @PlayerId";
        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@PlayerId", playerId);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Player
            {
                PlayerId = reader.GetInt32(reader.GetOrdinal("PlayerId")),
                PersonId = reader.GetInt32(reader.GetOrdinal("PersonId")),
                PlayerType = (PlayerType)reader.GetInt32(reader.GetOrdinal("PlayerType")),
                PlayerPosition = (PlayerPosition)reader.GetInt32(reader.GetOrdinal("PlayerPosition")),
                DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                PlayerStatus = (PlayerStatus)reader.GetInt32(reader.GetOrdinal("PlayerStatus"))

            };

        }

        return null;
    }

    public async Task<(List<PlayerView> Players, int TotalCount)> GetAllPlayersAsync(
        int? playerId,
        string? kfupmId,
        int pageNumber = 1,
        int pageSize = 10)
    {
        var players = new List<PlayerView>();
        int totalCount = 0;

        using var conn = new SqlConnection(_connection.ConnectionString);
        using var cmd = new SqlCommand("dbo.SP_SearchPlayers", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@PlayerId", (object?)playerId ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@KFUPMId", (object?)kfupmId ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
        cmd.Parameters.AddWithValue("@PageSize", pageSize);

        var totalParam = new SqlParameter("@TotalCount", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };
        cmd.Parameters.Add(totalParam);

        await conn.OpenAsync();

        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            players.Add(new PlayerView
            {
                PlayerId = reader.GetInt32(reader.GetOrdinal("PlayerId")),
                PersonId = reader.GetInt32(reader.GetOrdinal("PersonId")),
                PlayerType = reader.GetInt32(reader.GetOrdinal("PlayerType")),
                PlayerStatus = reader.GetInt32(reader.GetOrdinal("PlayerStatus")),
                DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                KFUPMId = reader.GetString(reader.GetOrdinal("KFUPMId")),
                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                SecondName = reader.IsDBNull(reader.GetOrdinal("SecondName")) ? null : reader.GetString(reader.GetOrdinal("SecondName")),
                ThirdName = reader.IsDBNull(reader.GetOrdinal("ThirdName")) ? null : reader.GetString(reader.GetOrdinal("ThirdName")),
                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                NationalityId = reader.GetInt32(reader.GetOrdinal("NationalityId"))
            });
        }
        reader.Close();
        totalCount = totalParam.Value != DBNull.Value ? (int)totalParam.Value : 0;

        return (players, totalCount);
    }


    public async Task<bool> ValidatePlayerTeamsInTeamAsync(
        int tournamentId,
        int tournamentTeamId,
        List<int> playerTeamIds)
    {
        using var connection = new SqlConnection(_connection.ConnectionString);
        using var command = new SqlCommand("SP_ValidatePlayerTeamsInTeam", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        // Add scalar parameters
        command.Parameters.AddWithValue("@TournamentId", tournamentId);
        command.Parameters.AddWithValue("@TournamentTeamId", tournamentTeamId);

        // Prepare table-valued parameter
        var tvpTable = CreatePlayerTeamIdTable(playerTeamIds);
        var tvpParam = command.Parameters.AddWithValue("@PlayerTeams", tvpTable);
        tvpParam.SqlDbType = SqlDbType.Structured;
        tvpParam.TypeName = "TVP_PlayerTeamIdList"; // ✅ use the new TVP type name

        // Add return value
        var returnValue = new SqlParameter
        {
            Direction = ParameterDirection.ReturnValue,
            SqlDbType = SqlDbType.Int
        };
        command.Parameters.Add(returnValue);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();

        return (int)returnValue.Value == 1;
    }


    public async Task<bool> AssignPlayerToTeamAsync(PlayerTeam playerTeam)
    {
        using var connection = new SqlConnection(_connection.ConnectionString);
        await connection.OpenAsync();



        using var command = new SqlCommand("SP_AssignPlayerToTeam", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@PlayerId", playerTeam.PlayerId);
        command.Parameters.AddWithValue("@TeamId", playerTeam.TeamId);
        command.Parameters.AddWithValue("@JoinedAt", DateTime.Now);
        command.Parameters.AddWithValue("@PlayerPosition", playerTeam.PlayerPosition);
        command.Parameters.AddWithValue("@PlayerRole", playerTeam.PlayerRole);

        await command.ExecuteNonQueryAsync();
        return true;

    }

    public async Task<bool> IsPlayerAlreadyAssignedAsync(int playerId, int tournamentId)
    {
        using var connection = new SqlConnection(_connection.ConnectionString);
        using var command = new SqlCommand("CheckPlayerAlreadyAssigned", connection);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@PlayerId", playerId);
        command.Parameters.AddWithValue("@TournamentId", tournamentId);

        await connection.OpenAsync();

        var result = await command.ExecuteScalarAsync();

        return result != null && Convert.ToBoolean(result);
    }


    public async Task<bool> IsUserAlreadyPlayerAsync(int userId)
    {
        using var connection = new SqlConnection(_connection.ConnectionString);
        await connection.OpenAsync();

        var query = @"
        SELECT 1
        FROM AspNetUsers U
        INNER JOIN People P ON U.PersonId = P.PersonId
        INNER JOIN Players PL ON PL.PersonId = P.PersonId
        WHERE U.Id = @UserId";

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@UserId", userId);

        var result = await command.ExecuteScalarAsync();
        return result != null;
    }


    private static DataTable CreatePlayerTeamIdTable(IEnumerable<int> playerTeamIds)
    {
        var table = new DataTable();
        table.Columns.Add("PlayerTeamId", typeof(int));

        foreach (var id in playerTeamIds)
        {
            table.Rows.Add(id);
        }

        return table;
    }
    public async Task<List<TopScorerPlayerView>> GetTopScorersAsync(int pageNumber, int pageSize)
    {
        var offset = (pageNumber - 1) * pageSize;
        var result = new List<TopScorerPlayerView>();

        var query = @"
            WITH CTE AS (
                SELECT 
                    PT.PlayerTeamId,
                    PE.FirstName,
                    PE.LastName, 
                    COUNT(*) AS NumberOfGoalsPerPlayer 
                FROM dbo.ShotsOnGoal 
                INNER JOIN dbo.PlayersTeams PT ON PT.PlayerTeamId = ShotsOnGoal.PlayerTeamId
                INNER JOIN dbo.Players PP ON PP.PlayerId = PT.PlayerId
                INNER JOIN dbo.People PE ON PE.PersonId = PP.PersonId
                WHERE IsGoal = 1
                GROUP BY PT.PlayerTeamId, PE.FirstName, PE.LastName
            ) 
            SELECT * FROM CTE 
            ORDER BY CTE.NumberOfGoalsPerPlayer DESC
            OFFSET @Offset ROWS
            FETCH NEXT @PageSize ROWS ONLY";

        using var connection = new SqlConnection(_connection.ConnectionString);
        using var command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@Offset", offset);
        command.Parameters.AddWithValue("@PageSize", pageSize);

        await connection.OpenAsync();

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            result.Add(new TopScorerPlayerView
            {
                PlayerTeamId = reader.GetInt32(reader.GetOrdinal("PlayerTeamId")),
                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                NumberOfGoalsPerPlayer = reader.GetInt32(reader.GetOrdinal("NumberOfGoalsPerPlayer"))
            });
        }

        return result;
    }



    public async Task<List<PlayerViolationView>> GetPlayerViolationsAsync(int pageNumber, int pageSize, int? cardType)
    {
        var offset = (pageNumber - 1) * pageSize;
        var result = new List<PlayerViolationView>();

        var query = @"
        SELECT 
            CV.ViolationId,
            T.Name AS TeamName,
            PE.FirstName,
            PE.LastName,
            CASE 
                WHEN CV.CardType = 1 THEN 'Red Card'
                ELSE 'Yellow'
            END AS CardType
        FROM dbo.CardViolations CV
        INNER JOIN dbo.MatchRecords MR ON MR.MatchRecordId = CV.MatchRecordId
        INNER JOIN dbo.MatchSchedules MH ON MH.MatchScheduleId = MR.MatchScheduleId
        INNER JOIN dbo.TournamentTeams TT ON TT.TournamentTeamId = MH.TournamentTeamIdA
        INNER JOIN dbo.Teams T ON T.TeamId = TT.TeamId
        INNER JOIN dbo.PlayersTeams PT ON PT.PlayerTeamId = CV.PlayerTeamId
        INNER JOIN dbo.Players P ON P.PlayerId = PT.PlayerId
        INNER JOIN dbo.People PE ON PE.PersonId = P.PersonId
        WHERE (@CardType IS NULL OR CV.CardType = @CardType)

        UNION ALL

        SELECT 
            CV.ViolationId,
            T.Name AS TeamName,
            PE.FirstName,
            PE.LastName,
            CASE 
                WHEN CV.CardType = 1 THEN 'Red Card'
                ELSE 'Yellow'
            END AS CardType
        FROM dbo.CardViolations CV
        INNER JOIN dbo.MatchRecords MR ON MR.MatchRecordId = CV.MatchRecordId
        INNER JOIN dbo.MatchSchedules MH ON MH.MatchScheduleId = MR.MatchScheduleId
        INNER JOIN dbo.TournamentTeams TT ON TT.TournamentTeamId = MH.TournamentTeamIdB
        INNER JOIN dbo.Teams T ON T.TeamId = TT.TeamId
        INNER JOIN dbo.PlayersTeams PT ON PT.PlayerTeamId = CV.PlayerTeamId
        INNER JOIN dbo.Players P ON P.PlayerId = PT.PlayerId
        INNER JOIN dbo.People PE ON PE.PersonId = P.PersonId
        WHERE (@CardType IS NULL OR CV.CardType = @CardType)

        ORDER BY ViolationId
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
    ";

        using var connection = new SqlConnection(_connection.ConnectionString);
        using var command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@Offset", offset);
        command.Parameters.AddWithValue("@PageSize", pageSize);
        command.Parameters.AddWithValue("@CardType", cardType ?? (object)DBNull.Value);

        await connection.OpenAsync();

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            result.Add(new PlayerViolationView
            {
                ViolationId = reader.GetInt32(reader.GetOrdinal("ViolationId")),
                TeamName = reader.GetString(reader.GetOrdinal("TeamName")),
                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                CardType = reader.GetString(reader.GetOrdinal("CardType"))
            });
        }

        return result;
    }


}

