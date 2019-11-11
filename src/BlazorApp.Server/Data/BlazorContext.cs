using BlazorApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Server.Data
{
    public class BlazorContext : DbContext
    {
        public BlazorContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<DatabaseRecord> DatabaseRecords { get; set; }
    }
}