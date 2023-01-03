using System.CommandLine;
using CodeWright.Common.Asp;
using CodeWright.Metadata.API;
using CodeWright.Metadata.API.Events;
using CodeWright.Metadata.API.Model;
using CodeWright.Metadata.Installer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var rootCommand = new RootCommand("Install and create the metadata databas");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var settings = new ServiceSettings(builder.Configuration);

// Log settings
using var loggerFactory = LoggerFactory.Create(config =>
{
    config.AddConsole();
    config.AddConfiguration(builder.Configuration.GetSection("Logging"));
});
var logger = loggerFactory.CreateLogger("Program");
settings.LogSettings(logger);

builder.Services.AddSingleton(settings);

builder.Services.AddInternalBus();

// Register metadata services
builder.Services.AddEntityFrameworkEventSourcing(settings.Database, settings.EventConnectionString);
builder.Services.AddMetadataViewDatabase(settings.Database, settings.ViewConnectionString);
builder.Services.AddMetadataCommands();
builder.Services.AddEvents<Item, ItemFactory>(typeof(ItemMetadataAddedEvent).Assembly);
builder.Services.AddMetadataQueries();
builder.Services.AddMetadataViewUpdaters();

var app = builder.Build();

rootCommand.SetHandler(async() =>
{
    await Install.MigrateAsync(app.Services.GetRequiredService<IServiceScopeFactory>());
});

return await rootCommand.InvokeAsync(args);
