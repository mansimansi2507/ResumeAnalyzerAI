using Microsoft.EntityFrameworkCore;
using ResumeAnalyzerAI.Data;
using Microsoft.EntityFrameworkCore.SqlServer; // Add this using directive

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

if (!string.IsNullOrEmpty(databaseUrl))
{
    // Railway PostgreSQL connection
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(databaseUrl));
}
else
{
    // Local SQL Server connection
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}
builder.Services.AddTransient<ResumeAnalyzerAI.Services.OpenAIService>();
var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Resume}/{action=Index}/{id?}");

app.Run();
