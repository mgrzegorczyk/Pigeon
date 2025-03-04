using Pigeon.Application.Extensions;
using Pigeon.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var connectionString = configuration.GetValue<string>("ConnectionString") ??
                       throw new Exception("Can't get connection string from APP configuration!");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// Add layers
builder.Services.AddInfrastructure(connectionString, configuration);
builder.Services.AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.UseApplication();
app.UseInfrastructure();

app.Run();