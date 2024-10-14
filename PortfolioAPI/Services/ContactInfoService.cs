using MySql.Data.MySqlClient;
using PortfolioAPI.Models;


namespace PortfolioAPI.Services
{
    public class ContactInfoService
    {
        private readonly string _connectionString;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public ContactInfoService(IConfiguration configuration)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {

#pragma warning disable CS8601 // Possible null reference assignment.
            _connectionString = configuration.GetConnectionString("DefaultConnection");
#pragma warning restore CS8601 // Possible null reference assignment.

        }

        public async Task AddContactAsync(ContactInfo contactInfo)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                var query = "INSERT INTO ContactInfo (Name, Email, Message) VALUES (@Name, @Email, @Message)";
                await connection.OpenAsync();

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", contactInfo.Name);
                    command.Parameters.AddWithValue("@Email", contactInfo.Email);
                    command.Parameters.AddWithValue("@Message", contactInfo.Message);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
