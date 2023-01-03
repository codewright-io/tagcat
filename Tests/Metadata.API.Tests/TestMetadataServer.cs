using CodeWright.Common.EventSourcing.EntityFramework;
using CodeWright.Metadata.API.Queries;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CodeWright.Metadata.API.Tests
{
    internal class TestMetadataServer : WebApplicationFactory<Program>
    {
        private readonly string _eventDbFile;
        private readonly string _viewDbFile;

        public TestMetadataServer()
        {
            // TODO: Need to cleanup after previous test runs
            // DeletePreviousDatabaseFiles();
            _eventDbFile = copyDatabase("meta");
            _viewDbFile = copyDatabase("view");
        }

        private static string copyDatabase(string database)
        {
            // Note: You need to have run the installer first to create the database
            var databaseFile = new FileInfo($"../../../../../Metadata.Installer/{database}.db");
            string outputFile = $"{database}-{Guid.NewGuid()}.db";
            databaseFile.CopyTo(outputFile, true);

            return outputFile;
        }

        private static void DeletePreviousDatabaseFiles()
        {
            // Delete database, plus any write ahead and temporary files that it produces
            // Note: If you don't delete the temp files, then it retains information from aborted tests

            var dir = new DirectoryInfo(Environment.CurrentDirectory);
            var databaseFiles = dir.EnumerateFiles("*.db*");

            foreach (var file in databaseFiles)
            {
                file.Delete();
            }
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
    }
}
