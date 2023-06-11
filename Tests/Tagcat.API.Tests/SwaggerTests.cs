using System.Net;
using FluentAssertions;

namespace CodeWright.Tagcat.API.Tests;

[Collection("Integration")]
public class SwaggerTests
{
    [Fact(DisplayName = "Get swagger")]
    public async Task GetSwaggerAsync()
    {
        await using var server = new TestMetadataServer();
        using var client = server.CreateClient();

        var response = await client.GetAsync(Get.SwaggerIndex());
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
