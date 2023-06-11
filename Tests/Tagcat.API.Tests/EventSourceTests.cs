using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using CodeWright.Common.EventSourcing;
using CodeWright.Common.EventSourcing.EntityFramework;
using CodeWright.Metadata.API.Commands;
using CodeWright.Metadata.API.Events;
using CodeWright.Metadata.API.Queries.Views;
using FluentAssertions;
using Newtonsoft.Json;

namespace CodeWright.Metadata.API.Tests;

[Collection("Integration")]
public class EventSourceTests
{
    private async Task<IEnumerable<IDomainEvent>> GetEventsAsync(HttpClient client, string tenantId, string targetId, int limit)
    {
        // Use the Newtonsoft parser because the default one struggles with enums as strings
        var stringResult = await client.GetStringAsync(Get.ItemEvents(tenantId, targetId, limit));
        var converter = new DomainEventJsonConverter(typeof(ItemMetadataAddedEvent).Assembly);
        var result = JsonConvert.DeserializeObject<IEnumerable<IDomainEvent>>(stringResult, converter);
        return result ?? throw new ApplicationException("Can't deserialize events");
    }

    [Fact(DisplayName = "Snapshot Performance Test")]
    public async Task SnapshotPerformanceTestAsync()
    {
        await using var server = new TestMetadataServer();
        using var client = server.CreateClient();

        // Create some unique tags
        var tagCommands = Enumerable.Range(1, 500)
            .Select(i => "tag" + i)
            .Select(id => new ItemTagsAddCommand
            {
                Id = "test",
                TenantId = "tenant",
                Tags = new List<string> { id }
            });

        // Send the commands
        var sw = Stopwatch.StartNew();
        foreach (var command in tagCommands) 
        {
            var response = await client.PostAsJsonAsync(Post.AddTags(), command);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        sw.Stop();
        sw.ElapsedMilliseconds.Should().BeLessThan(20000, "Took too long");

        // ASSERT: Check that all the tags were saved
        var tags = await client.GetFromJsonAsync<IEnumerable<ItemTagViewEntry>>(Get.ItemTags("tenant", "test"));
        Assert.NotNull(tags);
        tags.Count().Should().Be(tagCommands.Count());

        // ASSERT: Check that all the events were stored
        var events = await GetEventsAsync(client, "tenant", "test", 600);
        Assert.NotNull(events);
        events.Count().Should().Be(tagCommands.Count());
    }
}   