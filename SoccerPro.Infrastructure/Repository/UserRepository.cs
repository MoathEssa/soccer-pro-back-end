using Microsoft.Data.SqlClient;
using SoccerPro.Domain.Entities;
using SoccerPro.Domain.IRepository;
using System.Data;

public class UserRepository(IDbConnection connection) : IUserRepository
{

    private readonly IDbConnection _connection = connection;

    public async Task<User?> GetUserByIdOrUserName(int? UserId, string? UserName)
    {
        User? user = null;

        using (SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand("SP_GetUser", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", UserId);
                cmd.Parameters.AddWithValue("@UserName", UserName);


                await connection.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        user = new User
                        {
                            PersonId = reader.GetInt32(reader.GetOrdinal("PersonID")),
                        };
                    }
                }
            }
        }

        return user;
    }

}
