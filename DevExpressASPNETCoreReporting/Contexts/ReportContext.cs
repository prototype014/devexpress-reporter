using DevExpressASPNETCoreReporting.Models;
using Microsoft.EntityFrameworkCore;

namespace DevExpressASPNETCoreReporting.Data
{
    public class ReportContext : DbContext
    {
        public ReportContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Report> Reports { get; set; }
        public DbSet<Organisation> Organisation { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProdUnit> ProdUnit { get; set; }
        public DbSet<Unit> Unit { get; set; }
    }
}