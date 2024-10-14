using MySql.Data.MySqlClient;
using PortfolioAPI.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace PortfolioAPI.Services
{
    public class ProjectsService
    {
        private readonly string _connectionString;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public ProjectsService(IConfiguration configuration)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            _connectionString = configuration.GetConnectionString("DefaultConnection");
#pragma warning restore CS8601 // Possible null reference assignment.
        }

        public async Task<IEnumerable<Project>> GetProjectsAsync()
        {
            var projects = new List<Project>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Projects";
                await connection.OpenAsync();

                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            projects.Add(new Project
                            {
                                Id = reader.GetInt32("Id"),
                                Title = reader.GetString("Title"),
                                Description = reader.GetString("Description"),
                                Technologies = reader.GetString("Technologies"),
                                ImageUrl = reader.GetString("ImageUrl"),
                                Date = reader.GetDateTime("Date")
                            });
                        }
                    }
                }
            }
            return projects;
        }

        public async Task AddProjectAsync(Project project)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                var query = "INSERT INTO Projects (Title, Description, Technologies, ImageUrl, Date) VALUES (@Title, @Description, @Technologies, @ImageUrl, @Date)";
                await connection.OpenAsync();

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", project.Title);
                    command.Parameters.AddWithValue("@Description", project.Description);
                    command.Parameters.AddWithValue("@Technologies", project.Technologies);
                    command.Parameters.AddWithValue("@ImageUrl", project.ImageUrl);
                    command.Parameters.AddWithValue("@Date", project.Date);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateProjectAsync(int id, Project project)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                var query = "UPDATE Projects SET Title = @Title, Description = @Description, Technologies = @Technologies, ImageUrl = @ImageUrl, Date = @Date WHERE Id = @Id";
                await connection.OpenAsync();

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Title", project.Title);
                    command.Parameters.AddWithValue("@Description", project.Description);
                    command.Parameters.AddWithValue("@Technologies", project.Technologies);
                    command.Parameters.AddWithValue("@ImageUrl", project.ImageUrl);
                    command.Parameters.AddWithValue("@Date", project.Date);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteProjectAsync(int id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                var query = "DELETE FROM Projects WHERE Id = @Id";
                await connection.OpenAsync();

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
