using Microsoft.Data.Sqlite;
using LabManager.Models;
using LabManager.Database;
using Dapper;


namespace LabManager.Repositories;

class LabRepository
{
    private readonly DatabaseConfig _databaseConfig;

    public LabRepository(DatabaseConfig databaseConfig)
    {
        _databaseConfig = databaseConfig;
    }

    public IEnumerable<Lab> GetAll()
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);

        connection.Open();

        var lab = connection.Query<Lab>("SELECT * FROM Labs");

        return lab;
    }

    public Lab Save(Lab lab)
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);
        connection.Open();

        connection.Execute("INSERT INTO Labs VALUES(@Id, @Ram, @Processor)", lab);

        return lab;
    }

    public void Delete(int id)
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);
        connection.Open();

        connection.Execute("DELETE FROM Labs WHERE id = @Id", new {Id = id});
    }

    public Lab Update(Lab lab)
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);
        connection.Open();

        connection.Execute(@"
        UPDATE Labs 
        SET 
            ram = @ram,
            processor = @processor
        WHERE id = @id;
        ", lab);

        return lab;
    }

    public Lab GetById(int id)
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);
        connection.Open();

        var lab = connection.QuerySingle<Lab>("SELECT * FROM Labs WHERE id = @Id", new {Id = id});

        return lab;
    }

    public bool ExistsById(int id)
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);
        connection.Open();

        return connection.ExecuteScalar<bool>("SELECT COUNT(id) FROM Labs WHERE id = @Id", new {Id = id});
    }
} 