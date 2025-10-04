using Microsoft.EntityFrameworkCore;
using ResumeAnalyzerAI.Data;
using Microsoft.EntityFrameworkCore.SqlServer; // Add this using directive

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<ResumeAnalyzerAI.Services.OpenAIService>();
var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Resume}/{action=Index}/{id?}");

app.Run();
