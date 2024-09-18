using System.Data;

namespace FastProjects.Data.EntityFrameworkCore;

/// <summary>
/// Interface for a factory that creates SQL connections.
/// </summary>
public interface ISqlConnectionFactory
{
    /// <summary>
    /// Creates and returns a new instance of an <see cref="IDbConnection"/>.
    /// </summary>
    /// <returns>A new instance of an <see cref="IDbConnection"/>.</returns>
    IDbConnection CreateConnection();
}
