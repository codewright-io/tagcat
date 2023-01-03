using System.Net;
using System.Net.Http.Json;
using CodeWright.Metadata.API.Commands;
using CodeWright.Metadata.API.Model;
using CodeWright.Metadata.API.Queries;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting.Server;
using Newtonsoft.Json;

namespace CodeWright.Metadata.API.Tests;

[Collection("Integration")]
public class ReferenceTests
{
    private async Task<IEnumerable<ReferenceEntry>> GetReferencesAsync(HttpClient client, string tenantId, string id)
    {
        // Use the Newtonsoft parser because the default one struggles with enums as strings
        var stringResult = await client.GetStringAsync(Get.ItemReferences(tenantId, id));
        var result = JsonConvert.DeserializeObject<IEnumerable<ReferenceEntry>>(stringResult);
        return result ?? throw new ApplicationException("Can't deserialize references");
    }

    private async Task<IEnumerable<ItemResult>> GetReferencingAsync(HttpClient client, string tenantId, string targetId)
    {
        // Use the Newtonsoft parser because the default one struggles with enums as strings
        var stringResult = await client.GetStringAsync(Get.Referencing(tenantId, targetId));
        var result = JsonConvert.DeserializeObject<IEnumerable<ItemResult>>(stringResult);
        return result ?? throw new ApplicationException("Can't deserialize references");
    }

    [Fact(DisplayName = "Add, Remove, Set Reference")]
    public async Task AddRemoveSetReferenceAsync()
    {
        await using var server = new TestMetadataServer();
        using var client = server.CreateClient();

        // Test adding some references
        var addCommand = new ItemReferencesAddCommand
        { 
            Id = "test", 
            TenantId = "tenant", 
            References = new List<ReferenceEntry> 
            {
                new ReferenceEntry { TargetId="abc", Type = ReferenceType.AliasOf },
                new ReferenceEntry { TargetId="def", Type = ReferenceType.SubcategoryOf },
            } 
        };
        var response = await client.PostAsJsonAsync(Post.AddReferences(), addCommand);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        await Task.Delay(1000); // Wait for update

        var references = await GetReferencesAsync(client, "tenant", "test");
        references.Should().BeEquivalentTo(new List<ReferenceEntry> 
        {
            new ReferenceEntry { TargetId="abc", Type = ReferenceType.AliasOf },
            new ReferenceEntry { TargetId="def", Type = ReferenceType.SubcategoryOf },
        });

        // Test removing some references
        var removeCommand = new ItemReferencesRemoveCommand
        {
            Id = "test",
            TenantId = "tenant",
            References = new List<ReferenceEntry> { new ReferenceEntry { TargetId = "abc", Type = ReferenceType.AliasOf } },
        };
        response = await client.PostAsJsonAsync(Post.RemoveReferences(), removeCommand);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        await Task.Delay(1000); // Wait for update

        references = await GetReferencesAsync(client, "tenant", "test");
        references.Should().BeEquivalentTo(new List<ReferenceEntry>
        {
            new ReferenceEntry { TargetId="def", Type = ReferenceType.SubcategoryOf },
        });

        // Test adding some references
        addCommand = new ItemReferencesAddCommand
        {
            Id = "test",
            TenantId = "tenant",
            References = new List<ReferenceEntry>
            {
                new ReferenceEntry { TargetId="ghi", Type = ReferenceType.TranslationOf },
            }
        };
        response = await client.PostAsJsonAsync(Post.AddReferences(), addCommand);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        await Task.Delay(1000); // Wait for update

        references = await GetReferencesAsync(client, "tenant", "test");
        references.Should().BeEquivalentTo(new List<ReferenceEntry>
        {
            new ReferenceEntry { TargetId="ghi", Type = ReferenceType.TranslationOf },
            new ReferenceEntry { TargetId="def", Type = ReferenceType.SubcategoryOf },
        });

        // Test setting some references
        var setCommand = new ItemReferencesSetCommand
        {
            Id = "test",
            TenantId = "tenant",
            References = new List<ReferenceEntry> 
            {
                new ReferenceEntry { TargetId="jkl", Type = ReferenceType.RelatedTo },
                new ReferenceEntry { TargetId="mno", Type = ReferenceType.ChildOf },
            }
        };
        response = await client.PostAsJsonAsync(Post.SetReferences(), setCommand);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        await Task.Delay(1000); // Wait for update

        references = await GetReferencesAsync(client, "tenant", "test");
        references.Should().BeEquivalentTo(new List<ReferenceEntry>
        {
            new ReferenceEntry { TargetId="jkl", Type = ReferenceType.RelatedTo },
            new ReferenceEntry { TargetId="mno", Type = ReferenceType.ChildOf },
        });
    }

    [Fact(DisplayName = "Referencing Search")]
    public async Task ReferencingSearchAsync()
    {
        await using var server = new TestMetadataServer();
        using var client = server.CreateClient();

        // Test adding some references
        var addCommand = new ItemReferencesAddCommand
        {
            Id = "test1",
            TenantId = "tenant",
            References = new List<ReferenceEntry>
            {
                new ReferenceEntry { TargetId="test2", Type = ReferenceType.AliasOf },
            }
        };
        var response = await client.PostAsJsonAsync(Post.AddReferences(), addCommand);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        addCommand = new ItemReferencesAddCommand
        {
            Id = "test2",
            TenantId = "tenant",
            References = new List<ReferenceEntry>
            {
                new ReferenceEntry { TargetId="test1", Type = ReferenceType.AliasOf },
            }
        };
        response = await client.PostAsJsonAsync(Post.AddReferences(), addCommand);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        addCommand = new ItemReferencesAddCommand
        {
            Id = "test3",
            TenantId = "tenant",
            References = new List<ReferenceEntry>
            {
                new ReferenceEntry { TargetId="test1", Type = ReferenceType.EquivalentTo },
            }
        };
        response = await client.PostAsJsonAsync(Post.AddReferences(), addCommand);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        await Task.Delay(1000); // Wait for update

        var results = await GetReferencingAsync(client, "tenant", "test1");
        Assert.NotNull(results);
        results.Count().Should().Be(2);
    }
}   