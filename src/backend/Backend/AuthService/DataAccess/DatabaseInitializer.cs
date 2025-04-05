using Microsoft.EntityFrameworkCore;

namespace AuthService.DataAccess;

public class DatabaseInitializer(ApplicationContext context)
{
    public async Task Execute()
    {
        var migrations = await context.Database.GetPendingMigrationsAsync();
        if (migrations.Any())
        {
            await context.Database.MigrateAsync();
        }
    }
}
