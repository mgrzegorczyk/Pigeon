using Pigeon.API.Extensions;
using Pigeon.API.Hubs;
using Pigeon.Application.Extensions;
using Pigeon.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var connectionString = configuration.GetValue<string>("ConnectionString") ??
                       throw new Exception("Can't get connection string from APP configuration!");
var clientUrl = configuration.GetValue<string>("Client:Url") ??
                throw new Exception("Can't get client url from APP configuration!");


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddBearerSwagger();
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

app.UseCors(builder =>
    builder.WithOrigins(clientUrl)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());

app.MapHub<MessageHub>("/hubs/message");
app.MapControllers();

app.UseApplication();
app.UseInfrastructure();

app.Run();