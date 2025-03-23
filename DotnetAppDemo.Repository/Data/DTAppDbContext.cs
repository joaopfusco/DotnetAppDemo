using DotnetAppDemo.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DotnetAppDemo.Repository.Data
{
    public class DTAppDbContext : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Database=DotnetAppDemoDB;Username=postgres;Password=postgres");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
