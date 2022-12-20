using Microsoft.EntityFrameworkCore;
using TestAPI;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PerformanceContext>(options => 
	options.UseSqlServer(builder.Configuration.GetValue<string>("ConnectionString")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

/*
builder.WebHost.ConfigureKestrel(serverOptions =>
{
	serverOptions.Limits.MaxConcurrentConnections = 1000;
	serverOptions.Limits.MaxConcurrentUpgradedConnections = 1000;
});
*/

var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
