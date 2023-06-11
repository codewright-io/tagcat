using System.Net;
using System.Net.Http.Json;
using CodeWright.Tagcat.API.Commands;
using CodeWright.Tagcat.API.Queries.Views;
using FluentAssertions;

namespace CodeWright.Tagcat.API.Tests;

[Collection("Integration")]
public class TagTests
{
    [Fact(DisplayName = "Add, Remove, Set Tags")]
    public async Task AddRemoveSetTagsAsync()
    {
        await using var server = new TestMetadataServer();
        using var client = server.CreateClient();

        // Test adding some tags
        var addCommand = new ItemTagsAddCommand
        { 
            Id = "test", 
            TenantId = "tenant", 
            Tags = new List<string> { "White", "Eugene" } 
        };
        var response = await client.PostAsJsonAsync(Post.AddTags(), addCommand);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        await Task.Delay(1000); // Wait for update

        var tags = await client.GetFromJsonAsync<IEnumerable<ItemTagViewEntry>>(Get.ItemTags("tenant", "test"));
        tags.Should().BeEquivalentTo(new List<ItemTagViewEntry> 
        {
            new ItemTagViewEntry { DisplayName = "White" },
            new ItemTagViewEntry { DisplayName = "Eugene" },
        }, options => options.Excluding(p => p.Id));

        // Test removing some tags
        var removeCommand = new ItemTagsRemoveCommand
        {
            Id = "test",
            TenantId = "tenant",
            Tags = new List<string> { "White" }
        };
        response = await client.PostAsJsonAsync(Post.RemoveTags(), removeCommand);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        await Task.Delay(1000); // Wait for update

        tags = await client.GetFromJsonAsync<IEnumerable<ItemTagViewEntry>>(Get.ItemTags("tenant", "test"));
        tags.Should().BeEquivalentTo(new List<ItemTagViewEntry>
        {
            new ItemTagViewEntry { DisplayName = "Eugene" },
        }, options => options.Excluding(p => p.Id));

        // Test adding some tags
        addCommand = new ItemTagsAddCommand
        {
            Id = "test",
            TenantId = "tenant",
            Tags = new List<string> { "Blue" }
        };
        response = await client.PostAsJsonAsync(Post.AddTags(), addCommand);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        await Task.Delay(1000); // Wait for update

        tags = await client.GetFromJsonAsync<IEnumerable<ItemTagViewEntry>>(Get.ItemTags("tenant", "test"));
        tags.Should().BeEquivalentTo(new List<ItemTagViewEntry>
        {
            new ItemTagViewEntry { DisplayName = "Blue" },
            new ItemTagViewEntry { DisplayName = "Eugene" },
        }, options => options.Excluding(p => p.Id));

        // Test setting some tags
        var setCommand = new ItemTagsSetCommand
        {
            Id = "test",
            TenantId = "tenant",
            Tags = new List<string> { "Pink", "Chaotic" }
        };
        response = await client.PostAsJsonAsync(Post.SetTags(), setCommand);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        await Task.Delay(1000); // Wait for update

        tags = await client.GetFromJsonAsync<IEnumerable<ItemTagViewEntry>>(Get.ItemTags("tenant", "test"));
        tags.Should().BeEquivalentTo(new List<ItemTagViewEntry>
        {
            new ItemTagViewEntry { DisplayName = "Pink" },
            new ItemTagViewEntry { DisplayName = "Chaotic" },
        }, options => options.Excluding(p => p.Id));

        var searchResult = await client.GetFromJsonAsync<IEnumerable<ItemTagViewEntry>>(Get.ItemsByTag("tenant", "Pink"));
        Assert.NotNull(searchResult);
        searchResult.Single().Should().BeEquivalentTo(setCommand, options => options.ExcludingMissingMembers());
    }
}   