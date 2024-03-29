using System.Net;
using CodeWright.Common.Asp.Routes;
using FluentAssertions;

namespace CodeWright.Tagcat.API.Tests;

[Collection("Integration")]
public class SwaggerTests
{
    [Fact(DisplayName = "Get swagger")]
    public async Task GetSwaggerAsync()
    {
        await using var server = new TestTagcatServer();
        using var client = server.CreateClient();

        var response = await client.GetAsync(HttpGet.SwaggerIndex());
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
