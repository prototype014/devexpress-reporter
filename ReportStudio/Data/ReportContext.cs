using ReportStudio.Models;
using Microsoft.EntityFrameworkCore;

namespace ReportStudio.Data
{
    public class ReportContext : DbContext
    {
        public ReportContext(DbContextOptions<ReportContext> options) : base(options)
        {

        }

        public DbSet<Report> Reports { get; set; }
    }
}
