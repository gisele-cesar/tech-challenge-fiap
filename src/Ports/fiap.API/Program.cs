using fiap.Application;
using fiap.Repositories;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
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

builder.Services.AddTransient<Func<IDbConnection>>(sp => () =>
            new SqlConnection(builder.Configuration.GetConnectionString("fiap.sqlServer")));
builder.Services.AddRepositoriesModule();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opt =>
    {
        opt.SwaggerEndpoint("/swagger/v1/swagger.json", "FIAP - Tech Challenge V1");
    });
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
