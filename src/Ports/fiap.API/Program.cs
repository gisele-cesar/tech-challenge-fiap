using fiap.Application;
using fiap.Repositories;
using System.Data.SqlClient;
using System.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

/// Adiciona injeção de dependência no Application
builder.Services.AddApplicationModule();

builder.Services.AddTransient<Func<IDbConnection>>(sp => () =>
            new SqlConnection(builder.Configuration.GetConnectionString("fiap.sqlServer")));
builder.Services.AddRepositoriesModule();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecks("api/health");
    endpoints.MapControllers();
});

app.Run();
