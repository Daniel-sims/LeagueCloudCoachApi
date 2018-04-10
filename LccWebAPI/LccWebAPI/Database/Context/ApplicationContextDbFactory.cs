using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LccWebAPI.Database.Context
{
    public class ApplicationContextDbFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        DatabaseContext IDesignTimeDbContextFactory<DatabaseContext>.CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=LccDatabase;Trusted_Connection=True;ConnectRetryCount=0");

            return new DatabaseContext(optionsBuilder.Options);
        }
    }
}
