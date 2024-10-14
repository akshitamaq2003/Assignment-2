using MySql.Data.MySqlClient;
using PortfolioAPI.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace PortfolioAPI.Services
{
    public class SkillsService
    {
        private readonly string _connectionString;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public SkillsService(IConfiguration configuration)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            _connectionString = configuration.GetConnectionString("DefaultConnection");
#pragma warning restore CS8601 // Possible null reference assignment.
        }

        public async Task<IEnumerable<Skill>> GetSkillsAsync()
        {
            var skills = new List<Skill>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Skills";
                await connection.OpenAsync();

                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            skills.Add(new Skill
                            {
                                Id = reader.GetInt32("Id"),
                                SkillName = reader.GetString("SkillName"),
                                Proficiency = reader.GetString("Proficiency"),
                                LogoUrl = reader.GetString("LogoUrl")
                            });
                        }
                    }
                }
            }
            return skills;
        }

        // internal async Task AddSkillAsync(Skill skill)
        // {
        //     throw new NotImplementedException();
        // }
    }
}
