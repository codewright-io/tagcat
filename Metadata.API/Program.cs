using System.Text.Json.Serialization;
using CodeWright.Common.Asp;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Log settings
var logger = builder.CreateLogger("Program");
var settings = new ServiceSettings(builder.Configuration);
builder.Services.AddSingleton(settings);
settings.LogSettings(logger);

// Add controllers and configure enums to serialize as strings
builder.Services.AddControllers().AddJsonOptions(options => 
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

if (settings.ExposeSwaggerEndpoints)
{
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.AddXmlDocument();
    });
}

// Register metadata services
builder.Services.AddInternalBus();
builder.Services.AddAllMetadataService(settings);

// General HTTP services
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Check that the database have been created
await app.EnsureDatabaseExistsAsync();

// Add errors
app.UseExceptionHandler("/error");

// Configure the HTTP request pipeline.
if (settings.ExposeSwaggerEndpoints)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
