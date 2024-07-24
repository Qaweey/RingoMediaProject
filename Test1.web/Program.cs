using Serilog;
using Test.Web.Startup;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog((context, config) =>
{
    config.Enrich.FromLogContext()
        .WriteTo.Console()
        .ReadFrom.Configuration(context.Configuration);

});
builder.Services.RegisterServices(builder.Configuration);
var app = RequestPipeLines.App(builder);

app.Run();
