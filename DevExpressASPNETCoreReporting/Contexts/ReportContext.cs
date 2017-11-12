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
    }
}