using System.Net;
using System.Net.Http.Json;
using CodeWright.Tagcat.API.Commands;
using CodeWright.Tagcat.API.Model;
using CodeWright.Tagcat.API.Queries;
using FluentAssertions;

namespace CodeWright.Tagcat.API.Tests;

[Collection("Integration")]
public class MetadataTests
{
    [Fact(DisplayName = "Add, Remove, Set Metadata")]
    public async Task AddRemoveSetMetadataAsync()
    {
        await using var server = new TestMetadataServer();
        using var client = server.CreateClient();

        // Test adding some metadata
        var addCommand = new ItemMetadataAddCommand
        { 
            Id = "test", 
            TenantId = "tenant", 
            Metadata = new List<MetadataEntry> 
            {
                new MetadataEntry { Name = "Color", Value = "White" },
                new MetadataEntry { Name = "Author", Value = "Eugene" },
            } 
        };
        var response = await client.PostAsJsonAsync(Post.AddMetadata(), addCommand);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        await Task.Delay(1000); // Wait for update

        var metadata = await client.GetFromJsonAsync<IEnumerable<MetadataEntry>>(Get.ItemMetadata("tenant", "test"));
        metadata.Should().BeEquivalentTo(new List<MetadataEntry> 
        { 
            new MetadataEntry { Name = "Color", Value = "White" },
            new MetadataEntry { Name = "Author", Value = "Eugene" },
        });

        // Test removing some metadata
        var removeCommand = new ItemMetadataRemoveCommand
        {
            Id = "test",
            TenantId = "tenant",
            Metadata = new List<MetadataEntry> { new MetadataEntry { Name = "Color" } }
        };
        response = await client.PostAsJsonAsync(Post.RemoveMetadata(), removeCommand);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        await Task.Delay(1000); // Wait for update

        metadata = await client.GetFromJsonAsync<IEnumerable<MetadataEntry>>(Get.ItemMetadata("tenant", "test"));
        metadata.Should().BeEquivalentTo(new List<MetadataEntry>
        {
            new MetadataEntry { Name = "Author", Value = "Eugene" },
        });

        // Test adding some metadata
        addCommand = new ItemMetadataAddCommand
        {
            Id = "test",
            TenantId = "tenant",
            Metadata = new List<MetadataEntry>
            {
                new MetadataEntry { Name = "Color", Value = "Blue" },
            }
        };
        response = await client.PostAsJsonAsync(Post.AddMetadata(), addCommand);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        await Task.Delay(1000); // Wait for update

        metadata = await client.GetFromJsonAsync<IEnumerable<MetadataEntry>>(Get.ItemMetadata("tenant", "test"));
        metadata.Should().BeEquivalentTo(new List<MetadataEntry>
        {
            new MetadataEntry { Name = "Color", Value = "Blue" },
            new MetadataEntry { Name = "Author", Value = "Eugene" },
        });

        // Test setting some metadata
        var setCommand = new ItemMetadataSetCommand
        {
            Id = "test",
            TenantId = "tenant",
            Metadata = new List<MetadataEntry> 
            { 
                new MetadataEntry { Name = "Color", Value = "Pink" },
                new MetadataEntry { Name = "Alignment", Value = "Chaotic" },
            }
        };
        response = await client.PostAsJsonAsync(Post.SetMetadata(), setCommand);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        await Task.Delay(1000); // Wait for update

        metadata = await client.GetFromJsonAsync<IEnumerable<MetadataEntry>>(Get.ItemMetadata("tenant", "test"));
        metadata.Should().BeEquivalentTo(new List<MetadataEntry>
        {
            new MetadataEntry { Name = "Color", Value = "Pink" },
            new MetadataEntry { Name = "Alignment", Value = "Chaotic" },
        });
    }

    [Fact(DisplayName = "Search Metadata")]
    public async Task SearchMetadataAsync()
    {
        await using var server = new TestMetadataServer();
        using var client = server.CreateClient();

        // Add some metadata
        var setCommands = new ItemMetadataSetCommand[]
        {
            new ItemMetadataSetCommand
            {
                Id = "test1",
                TenantId = "tenant1",
                Metadata = new List<MetadataEntry>
                {
                    new MetadataEntry { Name = "Color", Value = "Blue" },
                    new MetadataEntry { Name = "Author", Value = "Eugene" },
                    new MetadataEntry { Name = "Animal", Value = "Dog" },
                    new MetadataEntry { Name = "Language", Value = "English" },
                }
            },
            new ItemMetadataSetCommand
            {
                Id = "test2",
                TenantId = "tenant1",
                Metadata = new List<MetadataEntry>
                {
                    new MetadataEntry { Name = "Color", Value = "Red" },
                    new MetadataEntry { Name = "Author", Value = "Paul" },
                    new MetadataEntry { Name = "Animal", Value = "Cat" },
                    new MetadataEntry { Name = "Language", Value = "English" },
                }
            },
            new ItemMetadataSetCommand
            {
                Id = "test3",
                TenantId = "tenant1",
                Metadata = new List<MetadataEntry>
                {
                    new MetadataEntry { Name = "Color", Value = "Blue" },
                    new MetadataEntry { Name = "Author", Value = "Derek" },
                    new MetadataEntry { Name = "Animal", Value = "Tiger" },
                    new MetadataEntry { Name = "Language", Value = "German" },
                }
            },
            new ItemMetadataSetCommand
            {
                Id = "test4",
                TenantId = "tenant2",
                Metadata = new List<MetadataEntry>
                {
                    new MetadataEntry { Name = "Color", Value = "Blue" },
                    new MetadataEntry { Name = "Author", Value = "Augustus" },
                    new MetadataEntry { Name = "Animal", Value = "Lion" },
                    new MetadataEntry { Name = "Language", Value = "English" },
                }
            }
        };

        foreach (var setCommand in setCommands)
        {
            var response = await client.PostAsJsonAsync(Post.SetMetadata(), setCommand);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        // Check for no results
        var results = await client.GetFromJsonAsync<IEnumerable<ItemResult>>(Get.SearchMetadata("tenant1", "Animal", "Lion", 20, 0));
        Assert.NotNull(results);
        results.Count().Should().Be(0);

        // Check for one result
        results = await client.GetFromJsonAsync<IEnumerable<ItemResult>>(Get.SearchMetadata("tenant1", "Animal", "Tiger", 20, 0));
        Assert.NotNull(results);
        results.Count().Should().Be(1);
        results.Should().BeEquivalentTo(setCommands.Where(c => c.Id == "test3"));

        // Check for two results
        results = await client.GetFromJsonAsync<IEnumerable<ItemResult>>(Get.SearchMetadata("tenant1", "Color", "Blue", 20, 0));
        Assert.NotNull(results); 
        results.Count().Should().Be(2);
        results.Should().BeEquivalentTo(setCommands.Where(c => c.Id != "test2" && c.TenantId == "tenant1"));

        // Check secondary filter
        results = await client.GetFromJsonAsync<IEnumerable<ItemResult>>(Get.SearchMetadata("tenant1", "Color", "Blue", "Language", "English", 20, 0));
        Assert.NotNull(results); 
        results.Count().Should().Be(1);
        results.Should().BeEquivalentTo(setCommands.Where(c => c.Id == "test1"));
    }
}   