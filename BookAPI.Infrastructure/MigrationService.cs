using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace BookAPI.Infrastructure
{
    public static class MigrationService
    {
        public static IApplicationBuilder MigrationDatabase(this IApplicationBuilder application)
        {
            using (var scope = application.ApplicationServices.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<DataContext>())
                {
                    context.Database.Migrate();
                }
            }
            return application;
        }
    }
}
