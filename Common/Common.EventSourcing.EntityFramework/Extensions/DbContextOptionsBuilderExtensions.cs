using CodeWright.Common;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder UseDatabase(this DbContextOptionsBuilder optionsBuilder, DatabaseType database, string connectionString, string migrationAssembly)
        {
            switch (database)
            {
                case DatabaseType.MicrosoftSQL:
                    optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly(migrationAssembly));
                    break;

                case DatabaseType.MySQL:
                    optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), b => b.MigrationsAssembly(migrationAssembly));
                    break;

                case DatabaseType.PostgreSQL:
                    optionsBuilder.UseNpgsql(connectionString, b => b.MigrationsAssembly(migrationAssembly));
                    break;

                case DatabaseType.SQLite:
                default:
                    optionsBuilder.UseSqlite(connectionString, b => b.MigrationsAssembly(migrationAssembly));
                    break;
            }

            return optionsBuilder;
        }
    }
}
