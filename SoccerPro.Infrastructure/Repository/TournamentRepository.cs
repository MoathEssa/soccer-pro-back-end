using Microsoft.Data.SqlClient;
using SoccerPro.Domain.Entities;
using SoccerPro.Domain.IRepository;
using System.Data;

namespace SoccerPro.Infrastructure.Repository
{
    public class TournamentRepository : ITournamentRepository
    {
        private readonly IDbConnection _connection;

        public TournamentRepository(IDbConnection connection) => _connection = connection;

        public async Task<int> AddTournamentAsync(Tournament tournament)
        {
            using var connection = new SqlConnection(_connection.ConnectionString);
            using var command = new SqlCommand("SP_InsertTournament", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@Name", tournament.Name);
            command.Parameters.AddWithValue("@StartDate", tournament.StartDate);
            command.Parameters.AddWithValue("@EndDate", tournament.EndDate);

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<bool> UpdateTournamentAsync(Tournament tournament)
        {
            using var connection = new SqlConnection(_connection.ConnectionString);
            using var command = new SqlCommand("SP_UpdateTournament", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@TournamentId", tournament.TournamentId);
            command.Parameters.AddWithValue("@Name", tournament.Name);
            command.Parameters.AddWithValue("@StartDate", tournament.StartDate);
            command.Parameters.AddWithValue("@EndDate", tournament.EndDate);

            await connection.OpenAsync();
            var rows = await command.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteTournamentAsync(int tournamentId)
        {
            using var connection = new SqlConnection(_connection.ConnectionString);
            using var command = new SqlCommand("SP_DeleteTournament", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@TournamentId", tournamentId);

            await connection.OpenAsync();
            var rows = await command.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<Tournament?> GetTournamentByIdAsync(int tournamentId)
        {
            using var connection = new SqlConnection(_connection.ConnectionString);
            using var command = new SqlCommand("SP_GetTournamentById", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@TournamentId", tournamentId);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Tournament
                {
                    TournamentId = reader.GetInt32(reader.GetOrdinal("TournamentId")),
                    Number = reader.GetString(reader.GetOrdinal("Number")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                    EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate"))
                };
            }

            return null;
        }

        public async Task<(List<Tournament> Tournaments, int TotalCount)> SearchTournamentsAsync(
            string? number, string? name, DateTime? startDate, DateTime? endDate, int pageNumber, int pageSize)
        {
            using var connection = new SqlConnection(_connection.ConnectionString);
            using var command = new SqlCommand("SP_SearchTournaments", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@Number", (object?)number ?? DBNull.Value);
            command.Parameters.AddWithValue("@Name", (object?)name ?? DBNull.Value);
            command.Parameters.AddWithValue("@StartDate", (object?)startDate ?? DBNull.Value);
            command.Parameters.AddWithValue("@EndDate", (object?)endDate ?? DBNull.Value);
            command.Parameters.AddWithValue("@PageNumber", pageNumber);
            command.Parameters.AddWithValue("@PageSize", pageSize);

            var totalCountParam = new SqlParameter("@TotalCount", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(totalCountParam);

            await connection.OpenAsync();

            var tournaments = new List<Tournament>();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                tournaments.Add(new Tournament
                {
                    TournamentId = reader.GetInt32(reader.GetOrdinal("TournamentId")),
                    Number = reader.GetString(reader.GetOrdinal("Number")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                    EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate"))
                });
            }

            var totalCount = (int)(totalCountParam.Value ?? 0);
            return (tournaments, totalCount);
        }

        public async Task<bool> AssignTeamToTournamentAsync(int tournamentId, int teamId)
        {
            using var connection = new SqlConnection(_connection.ConnectionString);
            using var command = new SqlCommand("AssignTeamToTournament", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@TournamentId", tournamentId);
            command.Parameters.AddWithValue("@TeamId", teamId);

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();

            return Convert.ToInt32(result) == 1;
        }


        public async Task<bool> TournamentExistsAsync(int tournamentId)
        {
            using var connection = new SqlConnection(_connection.ConnectionString);
            using var command = new SqlCommand("SP_CheckTournamentExists", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@TournamentId", tournamentId);

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();

            return Convert.ToInt32(result) == 1;

        }

        public async Task<bool> IsTeamInTournamentAsync(int teamId, int tournamentId)
        {
            using var connection = new SqlConnection(_connection.ConnectionString);
            using var command = new SqlCommand("CheckTeamInTournament", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@TeamId", teamId);
            command.Parameters.AddWithValue("@TournamentId", tournamentId);

            await connection.OpenAsync();

            var result = await command.ExecuteScalarAsync();

            return Convert.ToInt32(result) == 1;
        }

    }
}