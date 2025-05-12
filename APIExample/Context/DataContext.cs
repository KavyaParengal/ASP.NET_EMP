using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace APIExample.Context
{
    public class DataContext : DbContext
    {
        private readonly string _connectionString;
        public DataContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SQLConnection");
        }
        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
