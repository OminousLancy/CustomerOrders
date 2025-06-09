using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CustomerOrders;

public static class ApplicationBuilderExtensions
{
    public static async Task ApplyMigrationsAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();
            
        try
        {
            if (context.Database.GetPendingMigrations().Any())
            {
                logger.LogInformation("Applying EF Core migrations...");
                await context.Database.MigrateAsync();
                logger.LogInformation("EF Core migrations applied.");
            }
            else
            {
                logger.LogInformation("No pending migrations.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while applying migrations.");
            throw;
        }
    }
}