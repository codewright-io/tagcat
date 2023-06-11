using CodeWright.Common.EventSourcing.EntityFramework;
using CodeWright.Metadata.API.Queries;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CodeWright.Metadata.API.Tests;

internal class TestMetadataServer : WebApplicationFactory<Program>, IAsyncDisposable
{
    private readonly string _eventDbFile;
    private readonly string _viewDbFile;
    private bool _deletedDatabases = false;

    public TestMetadataServer()
    {
        // Create some new database files to run the tests with
        _eventDbFile = CopyDatabase("tag");
        _viewDbFile = CopyDatabase("tagview");
    }

    private static string CopyDatabase(string database)
    {
        // Note: You need to have run the installer first to create the database
        var databaseFile = new FileInfo($"../../../../../Tagcat.Installer/{database}.db");
        string outputFile = $"{database}-{Guid.NewGuid()}.db";
        databaseFile.CopyTo(outputFile, true);

        return outputFile;
    }

    public void DeleteDatabases()
    {
        if (_deletedDatabases) return;

        File.Delete(_eventDbFile);
        File.Delete(_viewDbFile);
        _deletedDatabases = true;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.ReplaceDbContext<EventSourceDbContext>($"Data Source={_eventDbFile}");
            services.ReplaceDbContext<MetadataDbContext>($"Data Source={_viewDbFile}");
        });

        return base.CreateHost(builder);
    }

    //public override async ValueTask DisposeAsync()
    //{
    //    var result = base.DisposeAsync();

    //    await Task.Delay(2000);
    //    DeleteDatabases();

    //    return;
    //}
}
