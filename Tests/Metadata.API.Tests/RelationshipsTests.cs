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
public class RelationshipsTests
{
    private async Task<IEnumerable<RelationshipEntry>> GetRelationshipsAsync(HttpClient client, string tenantId, string id)
    {
        // Use the Newtonsoft parser because the default one struggles with enums as strings
        var stringResult = await client.GetStringAsync(Get.ItemRelationships(tenantId, id));
        var result = JsonConvert.DeserializeObject<IEnumerable<RelationshipEntry>>(stringResult);
        return result ?? throw new ApplicationException("Can't de-serialize relationships");
    }

    private async Task<IEnumerable<ItemResult>> GetReferencingAsync(HttpClient client, string tenantId, string targetId)
    {
        // Use the Newtonsoft parser because the default one struggles with enums as strings
        var stringResult = await client.GetStringAsync(Get.Referencing(tenantId, targetId));
        var result = JsonConvert.DeserializeObject<IEnumerable<ItemResult>>(stringResult);
        return result ?? throw new ApplicationException("Can't de-serialize relationships");
    }

    [Fact(DisplayName = "Add, Remove, Set Relationships")]
    public async Task AddRemoveSetRelationshipsAsync()
    {
        await using var server = new TestMetadataServer();
        using var client = server.CreateClient();

        // Test adding some relationships
        var addCommand = new ItemRelationshipsAddCommand
        { 
            Id = "test", 
            TenantId = "tenant", 
            Relationships = new List<RelationshipEntry> 
            {
                new RelationshipEntry { TargetId="abc", Type = RelationshipType.AliasOf },
                new RelationshipEntry { TargetId="def", Type = RelationshipType.SubcategoryOf },
            } 
        };
        var response = await client.PostAsJsonAsync(Post.AddRelationships(), addCommand);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        await Task.Delay(1000); // Wait for update

        var relationships = await GetRelationshipsAsync(client, "tenant", "test");
        relationships.Should().BeEquivalentTo(new List<RelationshipEntry> 
        {
            new RelationshipEntry { TargetId="abc", Type = RelationshipType.AliasOf },
            new RelationshipEntry { TargetId="def", Type = RelationshipType.SubcategoryOf },
        });

        // Test removing some relationships
        var removeCommand = new ItemRelationshipsRemoveCommand
        {
            Id = "test",
            TenantId = "tenant",
            Relationships = new List<RelationshipEntry> { new RelationshipEntry { TargetId = "abc", Type = RelationshipType.AliasOf } },
        };
        response = await client.PostAsJsonAsync(Post.RemoveRelationships(), removeCommand);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        await Task.Delay(1000); // Wait for update

        relationships = await GetRelationshipsAsync(client, "tenant", "test");
        relationships.Should().BeEquivalentTo(new List<RelationshipEntry>
        {
            new RelationshipEntry { TargetId="def", Type = RelationshipType.SubcategoryOf },
        });

        // Test adding some relationships
        addCommand = new ItemRelationshipsAddCommand
        {
            Id = "test",
            TenantId = "tenant",
            Relationships = new List<RelationshipEntry>
            {
                new RelationshipEntry { TargetId="ghi", Type = RelationshipType.TranslationOf },
            }
        };
        response = await client.PostAsJsonAsync(Post.AddRelationships(), addCommand);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        await Task.Delay(1000); // Wait for update

        relationships = await GetRelationshipsAsync(client, "tenant", "test");
        relationships.Should().BeEquivalentTo(new List<RelationshipEntry>
        {
            new RelationshipEntry { TargetId="ghi", Type = RelationshipType.TranslationOf },
            new RelationshipEntry { TargetId="def", Type = RelationshipType.SubcategoryOf },
        });

        // Test setting some relationships
        var setCommand = new ItemRelationshipsSetCommand
        {
            Id = "test",
            TenantId = "tenant",
            Relationships = new List<RelationshipEntry> 
            {
                new RelationshipEntry { TargetId="jkl", Type = RelationshipType.RelatedTo },
                new RelationshipEntry { TargetId="mno", Type = RelationshipType.ChildOf },
            }
        };
        response = await client.PostAsJsonAsync(Post.SetReflationships(), setCommand);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        await Task.Delay(1000); // Wait for update

        relationships = await GetRelationshipsAsync(client, "tenant", "test");
        relationships.Should().BeEquivalentTo(new List<RelationshipEntry>
        {
            new RelationshipEntry { TargetId="jkl", Type = RelationshipType.RelatedTo },
            new RelationshipEntry { TargetId="mno", Type = RelationshipType.ChildOf },
        });
    }

    [Fact(DisplayName = "Referencing Search")]
    public async Task ReferencingSearchAsync()
    {
        await using var server = new TestMetadataServer();
        using var client = server.CreateClient();

        // Test adding some relationships
        var addCommand = new ItemRelationshipsAddCommand
        {
            Id = "test1",
            TenantId = "tenant",
            Relationships = new List<RelationshipEntry>
            {
                new RelationshipEntry { TargetId="test2", Type = RelationshipType.AliasOf },
            }
        };
        var response = await client.PostAsJsonAsync(Post.AddRelationships(), addCommand);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        addCommand = new ItemRelationshipsAddCommand
        {
            Id = "test2",
            TenantId = "tenant",
            Relationships = new List<RelationshipEntry>
            {
                new RelationshipEntry { TargetId="test1", Type = RelationshipType.AliasOf },
            }
        };
        response = await client.PostAsJsonAsync(Post.AddRelationships(), addCommand);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        addCommand = new ItemRelationshipsAddCommand
        {
            Id = "test3",
            TenantId = "tenant",
            Relationships = new List<RelationshipEntry>
            {
                new RelationshipEntry { TargetId="test1", Type = RelationshipType.EquivalentTo },
            }
        };
        response = await client.PostAsJsonAsync(Post.AddRelationships(), addCommand);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        await Task.Delay(1000); // Wait for update

        var results = await GetReferencingAsync(client, "tenant", "test1");
        Assert.NotNull(results);
        results.Count().Should().Be(2);
    }
}   