using Microsoft.Data.SqlClient;
using SoccerPro.Domain.Entities;
using SoccerPro.Domain.Entities.Enums;
using SoccerPro.Domain.Entities.Views;
using SoccerPro.Domain.IRepository;
using System.Data;

namespace SoccerPro.Infrastructure.Repository
{
    public class TeamRepository : ITeamRepository
    {
        private readonly IDbConnection _connection;

        public TeamRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<bool> AddTeamAsync(Team team)
        {
            using var connection = new SqlConnection(_connection.ConnectionString);
            using var command = new SqlCommand("SP_InsertTeam", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@Name", team.Name);
            command.Parameters.AddWithValue("@Address", team.Address);
            command.Parameters.AddWithValue("@Website", team.Website);
            command.Parameters.AddWithValue("@NumberOfPlayers", team.NumberOfPlayers);
            command.Parameters.AddWithValue("@ManagerId", team.ManagerId);

            // ✅ Build TVP for ContactInfos
            var contactInfosTable = new DataTable();
            contactInfosTable.Columns.Add("ContactType", typeof(int));     // as int
            contactInfosTable.Columns.Add("Value", typeof(string));        // as string

            foreach (var ci in team.TeamContactInfo)
            {
                contactInfosTable.Rows.Add(ci.ContactType, ci.Value);
            }

            var contactInfosParam = command.Parameters.AddWithValue("@ContactInfos", contactInfosTable);
            contactInfosParam.SqlDbType = SqlDbType.Structured;
            contactInfosParam.TypeName = "TeamContactInfoTVP";

            // Output param for new TeamId
            var outputId = new SqlParameter("@TeamId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(outputId);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();

            var id = (int?)outputId.Value;
            return id > 0;
        }

        public async Task<bool> DeleteTeamAsync(int teamId)
        {
            using var connection = new SqlConnection(_connection.ConnectionString);
            using var command = new SqlCommand("SP_DeleteTeam", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@TeamId", teamId);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            return true;
        }

        public async Task<(List<TeamView> teams, int totalCount)> SearchTeamsAsync(
            string? name = null,
            string? address = null,
            string? website = null,
            int? numberOfPlayers = null,
            int? managerId = null,
            string? managerFirstName = null,
            string? managerLastName = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            using var connection = new SqlConnection(_connection.ConnectionString);
            using var command = new SqlCommand("SP_SearchTeams", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@Name", (object?)name ?? DBNull.Value);
            command.Parameters.AddWithValue("@PageNumber", pageNumber);
            command.Parameters.AddWithValue("@PageSize", pageSize);

            var totalCountParam = new SqlParameter("@TotalCount", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(totalCountParam);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            var teams = new List<TeamView>();
            while (await reader.ReadAsync())
            {
                teams.Add(new TeamView
                {
                    TeamId = reader.GetInt32(reader.GetOrdinal("TeamId")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Address = reader.GetString(reader.GetOrdinal("Address")),
                    Website = reader.GetString(reader.GetOrdinal("Website")),
                    NumberOfPlayers = reader.GetInt32(reader.GetOrdinal("NumberOfPlayers")),
                    ManagerId = reader.GetInt32(reader.GetOrdinal("ManagerId")),
                    ManagerFirstName = reader.GetString(reader.GetOrdinal("ManagerFirstName")),
                    ManagerLastName = reader.GetString(reader.GetOrdinal("ManagerLastName"))

                });
            }

            return (teams, teams.Count);
        }

        public async Task<TeamView?> GetTeamByIdAsync(int teamId)
        {
            using var connection = new SqlConnection(_connection.ConnectionString);
            using var command = new SqlCommand("SP_GetTeamById", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@TeamId", teamId);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            TeamView? team = null;

            // First result: Team + Manager Info
            if (await reader.ReadAsync())
            {
                team = new TeamView
                {
                    TeamId = reader.GetInt32(reader.GetOrdinal("TeamId")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Address = reader.GetString(reader.GetOrdinal("Address")),
                    Website = reader.GetString(reader.GetOrdinal("Website")),
                    NumberOfPlayers = reader.GetInt32(reader.GetOrdinal("NumberOfPlayers")),
                    ManagerId = reader.GetInt32(reader.GetOrdinal("ManagerId")),
                    ManagerFirstName = reader.GetString(reader.GetOrdinal("ManagerFirstName")),
                    ManagerLastName = reader.GetString(reader.GetOrdinal("ManagerLastName")),
                    teamContactInfos = new List<TeamContactInfo>()
                };
            }

            // Second result: Contact Info
            if (await reader.NextResultAsync() && team != null)
            {
                while (await reader.ReadAsync())
                {
                    team.teamContactInfos.Add(new TeamContactInfo
                    {
                        ContactType = (ContactType)reader.GetInt32(reader.GetOrdinal("ContactType")),
                        Value = reader.GetString(reader.GetOrdinal("Value"))
                    });
                }
            }

            return team;
        }

        public async Task<bool> UpdateTeamAsync(Team team)
        {
            using var connection = new SqlConnection(_connection.ConnectionString);
            using var command = new SqlCommand("SP_UpdateTeam", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@TeamId", team.TeamId);
            command.Parameters.AddWithValue("@Name", team.Name);
            command.Parameters.AddWithValue("@Address", team.Address);
            command.Parameters.AddWithValue("@Website", team.Website);
            command.Parameters.AddWithValue("@NumberOfPlayers", team.NumberOfPlayers);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            return true;
        }


        public async Task<bool> TeamExistsAsync(int teamId)
        {
            using var connection = new SqlConnection(_connection.ConnectionString);
            using var command = new SqlCommand("SP_CheckTeamExists", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@TeamId", teamId);

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

        public async Task<bool> IsTeamInTournamentAsync(int TournamentTeamId)
        {
            using var connection = new SqlConnection(_connection.ConnectionString);
            using var command = new SqlCommand("SP_CheckTeamInTournamentByTournamentTeamId", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@TournamentTeamId", TournamentTeamId);

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();

            return Convert.ToInt32(result) == 1;
        }

        public async Task<(List<TeamTournamentView> teams, int totalCount)> GetTeamsByTournamentIdAsync(
            int tournamentId,
            int pageNumber = 1,
            int pageSize = 10)
        {
            using var connection = new SqlConnection(_connection.ConnectionString);
            using var command = new SqlCommand("SP_GetTeamsByTournament", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@TournamentId", tournamentId);
            command.Parameters.AddWithValue("@PageNumber", pageNumber);
            command.Parameters.AddWithValue("@PageSize", pageSize);



            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            var teams = new List<TeamTournamentView>();
            while (await reader.ReadAsync())
            {
                teams.Add(new TeamTournamentView
                {
                    TournamentTeamId = reader.GetInt32(reader.GetOrdinal("TournamentTeamId")),
                    TeamId = reader.GetInt32(reader.GetOrdinal("TeamId")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Address = reader.GetString(reader.GetOrdinal("Address")),
                    Website = reader.GetString(reader.GetOrdinal("Website")),
                    NumberOfPlayers = reader.GetInt32(reader.GetOrdinal("NumberOfPlayers")),
                    ManagerId = reader.GetInt32(reader.GetOrdinal("ManagerId")),
                    ManagerFirstName = reader.GetString(reader.GetOrdinal("ManagerFirstName")),
                    ManagerLastName = reader.GetString(reader.GetOrdinal("ManagerLastName"))
                });
            }

            return (teams, teams.Count);
        }


    }
}
