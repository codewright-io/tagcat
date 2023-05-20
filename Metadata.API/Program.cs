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
        string descriptionPath = Path.Combine(Environment.CurrentDirectory, "../openapi.md");
        options.AddXmlDocument();
        //options.AddXmlDocument(typeof(CommandResult).Assembly);
        options.IncludeDocumentInformation(
            title: "Tagcat",
            version: "v1",
            description: !string.IsNullOrEmpty(descriptionPath) && File.Exists(descriptionPath) ?
                File.ReadAllText(descriptionPath) : "Tagcat Description",
            logoUrl: new Uri("https://raw.githubusercontent.com/codewright-io/tagcat/main/tagcat_sml.png"),
            contactName: "Code Wright",
            contactUrl: new Uri("https://codewright.io/"),
            contactEmail: "admin@codewright.io",
            licenseUrl: new Uri("https://raw.githubusercontent.com/codewright-io/tagcat/main/LICENSE")
            );
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
