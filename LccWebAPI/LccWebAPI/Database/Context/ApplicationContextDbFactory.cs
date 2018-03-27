using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LccWebAPI.Database.Context
{
    public class ApplicationContextDbFactory : IDesignTimeDbContextFactory<LccDatabaseContext>
    {
        LccDatabaseContext IDesignTimeDbContextFactory<LccDatabaseContext>.CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LccDatabaseContext>();
            optionsBuilder.UseSqlServer<LccDatabaseContext>(@"Server=(localdb)\mssqllocaldb;Database=LccDatabase;Trusted_Connection=True;ConnectRetryCount=0");

            return new LccDatabaseContext(optionsBuilder.Options);
        }
    }
}
