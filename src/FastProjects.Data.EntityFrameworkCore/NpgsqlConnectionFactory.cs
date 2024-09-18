using System.Data;
using Npgsql;

namespace FastProjects.Data.EntityFrameworkCore;

/// <summary>
/// Factory for creating Npgsql database connections.
/// </summary>
/// <param name="connectionString">The connection string used to create the connection.</param>
public sealed class NpgsqlConnectionFactory(string connectionString) : ISqlConnectionFactory
{
    /// <inheritdoc />
    public IDbConnection CreateConnection()
    {
        var connection = new NpgsqlConnection(connectionString);
        connection.Open();
        return connection;
    }
}
