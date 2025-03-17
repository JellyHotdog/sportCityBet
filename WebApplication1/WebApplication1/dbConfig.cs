using Npgsql;
namespace WebApplication1
{
    public class dbConfig
    {
        public NpgsqlConnection dbConn = new NpgsqlConnection("Host=ep-rough-art-a8u0mf5s-pooler.eastus2.azure.neon.tech;Database=testdb;Username=testdb_owner;Password=npg_2reiEYtw0MRB");
    }
}
