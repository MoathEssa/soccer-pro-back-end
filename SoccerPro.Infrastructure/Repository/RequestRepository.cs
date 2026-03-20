using Microsoft.Data.SqlClient;
using SoccerPro.Domain.Entities;
using SoccerPro.Domain.Entities.Enums;
using SoccerPro.Domain.IRepository;
using System.Data;

namespace SoccerPro.Infrastructure.Repository;

public class RequestRepository : IRequestRepository
{
    private readonly IDbConnection _connection;

    public RequestRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<bool> CreateRequestAsync(JoinTeamForFirstTimeRequest joinTeamForFirstTimeRequest)
    {
        using var connection = new SqlConnection(_connection.ConnectionString);
        using var command = new SqlCommand("SP_CreateJoinTeamRequest", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@UserId", joinTeamForFirstTimeRequest.UserId);
        command.Parameters.AddWithValue("@PlayerPosition", (int)joinTeamForFirstTimeRequest.PlayerPosition);
        command.Parameters.AddWithValue("@PlayerType", (int)joinTeamForFirstTimeRequest.PlayerType);
        command.Parameters.AddWithValue("@PlayerRole", (int)joinTeamForFirstTimeRequest.PlayerRole);
        command.Parameters.AddWithValue("@DepratmentId", joinTeamForFirstTimeRequest.DepartmentId);
        command.Parameters.AddWithValue("@TeamId", joinTeamForFirstTimeRequest.TeamId);
        command.Parameters.AddWithValue("@Notes", (object?)joinTeamForFirstTimeRequest.Notes ?? DBNull.Value);

        var outputId = new SqlParameter("@RequestId", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };

        command.Parameters.Add(outputId);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();


        return true;
    }

    public async Task<bool> ProcessRequestJoinTeamForFirstTimeAsync(int requestId, int processorUserId, RequestStatus requestStatus, PlayerStatus playerStatus)
    {

        using var connection = new SqlConnection(_connection.ConnectionString);
        using var command = new SqlCommand("SP_ProcessRequestJoinTeamForFirstTime", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        // Input parameters
        command.Parameters.AddWithValue("@RequestId", requestId);
        command.Parameters.AddWithValue("@ProcessorUserId", processorUserId);
        command.Parameters.AddWithValue("@ReuestStatus", requestStatus); // Keep typo if SP name uses this
        command.Parameters.AddWithValue("@PlayerStatus", playerStatus);

        // Output parameter
        var successParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
        {
            Direction = ParameterDirection.Output
        };
        command.Parameters.Add(successParam);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();

        return successParam.Value != DBNull.Value && (bool)successParam.Value;
    }

    public async Task<bool> HasPendingTeamRequestAsync(int userId)
    {
        using var connection = new SqlConnection(_connection.ConnectionString);
        using var command = new SqlCommand("SP_HasPendingTeamRequest", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@UserId", userId);

        var outputParam = new SqlParameter("@HasPending", SqlDbType.Bit)
        {
            Direction = ParameterDirection.Output
        };
        command.Parameters.Add(outputParam);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();

        return (bool)outputParam.Value;
    }



    //------------------
    public async Task<Request?> GetRequestByIdAsync(int requestId)
    {
        using var connection = new SqlConnection(_connection.ConnectionString);
        using var command = new SqlCommand("SP_GetRequestById", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@RequestId", requestId);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Request
            {
                RequestId = requestId,
                RequestType = (RequestType)reader.GetInt32(reader.GetOrdinal("RequestType")),
                Status = (RequestStatus)reader.GetInt32(reader.GetOrdinal("Status")),
                ProcessedAt = reader.IsDBNull(reader.GetOrdinal("ProcessedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("ProcessedAt")),
                ProcessedByUserId = reader.IsDBNull(reader.GetOrdinal("ProcessedByUserId")) ? null : reader.GetString(reader.GetOrdinal("ProcessedByUserId")),
            };
        }

        return null;
    }

    public async Task<bool> IsPlayerInTeamAsync(int playerId, int teamId)
    {
        using var connection = new SqlConnection(_connection.ConnectionString);
        using var command = new SqlCommand("SP_IsPlayerInTeam", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@PlayerId", playerId);
        command.Parameters.AddWithValue("@TeamId", teamId);

        var outputParam = new SqlParameter("@IsInTeam", SqlDbType.Bit)
        {
            Direction = ParameterDirection.Output
        };
        command.Parameters.Add(outputParam);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();

        return (bool)outputParam.Value;
    }

    public async Task<(List<Request> Requests, int TotalCount)> GetRequestsByPlayerAsync(
        int playerId,
        int pageNumber = 1,
        int pageSize = 10)
    {
        using var connection = new SqlConnection(_connection.ConnectionString);
        using var command = new SqlCommand("SP_GetRequestsByPlayer", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@PlayerId", playerId);
        command.Parameters.AddWithValue("@PageNumber", pageNumber);
        command.Parameters.AddWithValue("@PageSize", pageSize);

        var totalCountParam = new SqlParameter("@TotalCount", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };
        command.Parameters.Add(totalCountParam);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();

        var requests = new List<Request>();
        while (await reader.ReadAsync())
        {
            requests.Add(new Request
            {
                RequestId = reader.GetInt32(reader.GetOrdinal("RequestId")),
                RequestType = (RequestType)reader.GetInt32(reader.GetOrdinal("RequestType")),
                Status = (RequestStatus)reader.GetInt32(reader.GetOrdinal("Status")),
                ProcessedAt = reader.IsDBNull(reader.GetOrdinal("ProcessedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("ProcessedAt")),
                ProcessedByUserId = reader.IsDBNull(reader.GetOrdinal("ProcessedByUserId")) ? null : reader.GetString(reader.GetOrdinal("ProcessedByUserId")),
            });
        }

        return (requests, (int)totalCountParam.Value);
    }

    public async Task<(List<Request> Requests, int TotalCount)> GetRequestsByTeamAsync(
        int teamId,
        int pageNumber = 1,
        int pageSize = 10)
    {
        using var connection = new SqlConnection(_connection.ConnectionString);
        using var command = new SqlCommand("SP_GetRequestsByTeam", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@TeamId", teamId);
        command.Parameters.AddWithValue("@PageNumber", pageNumber);
        command.Parameters.AddWithValue("@PageSize", pageSize);

        var totalCountParam = new SqlParameter("@TotalCount", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };
        command.Parameters.Add(totalCountParam);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();

        var requests = new List<Request>();
        while (await reader.ReadAsync())
        {
            requests.Add(new Request
            {
                RequestId = reader.GetInt32(reader.GetOrdinal("RequestId")),
                RequestType = (RequestType)reader.GetInt32(reader.GetOrdinal("RequestType")),
                Status = (RequestStatus)reader.GetInt32(reader.GetOrdinal("Status")),
                ProcessedAt = reader.IsDBNull(reader.GetOrdinal("ProcessedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("ProcessedAt")),
                ProcessedByUserId = reader.IsDBNull(reader.GetOrdinal("ProcessedByUserId")) ? null : reader.GetString(reader.GetOrdinal("ProcessedByUserId")),
            });
        }

        return (requests, (int)totalCountParam.Value);
    }
}