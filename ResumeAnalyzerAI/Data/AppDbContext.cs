using Microsoft.EntityFrameworkCore;
using ResumeAnalyzerAI.Models;

namespace ResumeAnalyzerAI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ResumeRecord> ResumeRecords { get; set; }
    }
}
