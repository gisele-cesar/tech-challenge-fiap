using fiap.Application;
using fiap.Domain.Services.Interfaces;
using fiap.Repositories;
using fiap.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Serilog;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "FIAP - Tech Challenge", Version = "v1" });
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
    });
builder.Services.AddHealthChecks();

// Adiciona injeção de dependência no Application
builder.Services.AddApplicationModule();
builder.Services.AddServicesModule();

//builder.Services.AddTransient<Func<IDbConnection>>(sp => () =>
//            new SqlConnection(builder.Configuration.GetConnectionString("fiap.sqlServer")));

builder.Services.AddSingleton<Func<IDbConnection>>( sp => { 
    var configuration = sp.GetRequiredService<IConfiguration>(); 
    var connectionString = configuration.GetConnectionString("fiap.sqlServer");
    var secretService = sp.GetRequiredService<ISecretManagerService>();

    var secret = secretService.ObterSecret("dev/fiap/sql-rds").Result;

    if (secret.Host is null)
    {
        Console.WriteLine("Não foi possível recuperar a secret - Console.WriteLine"); ;
        Log.Information("Não foi possível recuperar a secret Serilog");
        throw new Exception("Não foi possível recuperar a secret - Lançada excecao"); 
    }

    connectionString = connectionString
    .Replace("__server__", secret.Host)
    .Replace("__port__", secret.Port)
    .Replace("__db__", secret.DbInstanceIdentifier)
    .Replace("__userdb__", secret.UserName)
    .Replace("__senha__", secret.Password);

    return () => new SqlConnection(connectionString); 
});


builder.Services.AddRepositoriesModule();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(opt =>
{
    opt.SwaggerEndpoint("/swagger/v1/swagger.json", "FIAP - Tech Challenge V1");
});


app.UseHttpsRedirection();
app.UseSerilogRequestLogging();
app.UseAuthorization();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecks("api/health");
    endpoints.MapHealthChecks("api/metrics");
    endpoints.MapGet("api/stress", () =>
                    {
                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Start();

                        while (true)
                        {
                            /// Realiza cálculos intensivos para estressar a CPU
                            double result = Math.Pow(Math.PI, Math.E);

                            if (stopwatch.ElapsedMilliseconds > 60000)
                            {
                                Console.WriteLine($"saindo do calculo {Dns.GetHostName()}");
                                break;
                            }
                        }

                        return "ok";
                    });

    endpoints.MapControllers();
});

Log.Information("Iniciando aplicação");
app.Run();


