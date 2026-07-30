using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context.Domains.Network;
using ModularPipelines.Http;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using ModularPipelines.UnitTests.Helpers;
using NReco.Logging.File;
using File = System.IO.File;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace ModularPipelines.UnitTests.Context;

public class HttpTests : TestBase
{
    [Test]
    public async Task PublicApi_DoesNotExposeRawHttpClients()
    {
        var rawClientMembers = typeof(IHttpContext)
            .GetMembers()
            .Where(member => member switch
            {
                PropertyInfo property => property.PropertyType == typeof(HttpClient),
                MethodInfo method => method.ReturnType == typeof(HttpClient),
                _ => false,
            })
            .Select(member => member.Name);

        await Assert.That(rawClientMembers).IsEmpty();
    }

    [Test]
    public async Task Can_Send_Request_With_String_To_Request_Implicit_Conversion()
    {
        await using var server = LocalHttpServer.Start();
        var (http, _) = await GetService<IHttpContext>((_, _) => { });

        await http.SendAsync(server.Uri);
    }

    [Test]
    public async Task When_Log_Request_False_Then_Do_Not_Log_Request()
    {
        await using var server = LocalHttpServer.Start();
        var file = Path.Combine(TestContext.WorkingDirectory, Guid.NewGuid().ToString("N") + ".txt");

        var (http, host) = await GetService<IHttpContext>((_, collection) =>
        {
            collection.AddLogging(builder =>
            {
                collection.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);
                builder.AddFile(file);
            });
        });

        await http.SendAsync(new HttpOptions(new HttpRequestMessage(HttpMethod.Get, server.Uri))
        {
            ThrowOnNonSuccessStatusCode = false,
            LoggingType = HttpLoggingType.Response,
        });

        await host.DisposeAsync();

        var logFile = await File.ReadAllTextAsync(file);
        await Assert.That(logFile).DoesNotContain("HTTP Request:");
        await Assert.That(logFile).DoesNotContain($"GET {server.Uri} HTTP/1.1");
        await Assert.That(logFile).Contains("HTTP Response:");
        await Assert.That(logFile).Contains("Server: LocalHttpServer");
    }

    [Test]
    public async Task When_Log_Response_False_Then_Do_Not_Log_Response()
    {
        await using var server = LocalHttpServer.Start();
        var file = Path.Combine(TestContext.WorkingDirectory, Guid.NewGuid().ToString("N") + ".txt");

        var (http, host) = await GetService<IHttpContext>((_, collection) =>
        {
            collection.AddLogging(builder =>
            {
                collection.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);
                builder.AddFile(file);
            });
        });

        await http.SendAsync(new HttpOptions(new HttpRequestMessage(HttpMethod.Get, server.Uri))
        {
            ThrowOnNonSuccessStatusCode = false,
            LoggingType = HttpLoggingType.Request,
        });

        await host.DisposeAsync();

        var logFile = await File.ReadAllTextAsync(file);
        await Assert.That(logFile).Contains("HTTP Request:");
        await Assert.That(logFile).Contains($"GET {server.Uri} HTTP/1.1");
        await Assert.That(logFile).DoesNotContain("HTTP Response:");
        await Assert.That(logFile).DoesNotContain("Server: LocalHttpServer");
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async Task Assert_SendAsync_Logs_As_Expected(bool customHttpClient)
    {
        await using var server = LocalHttpServer.Start();
        var file = Path.Combine(TestContext.WorkingDirectory, Guid.NewGuid().ToString("N") + ".txt");

        var (http, host) = await GetService<IHttpContext>((_, collection) =>
        {
            collection.AddLogging(builder =>
            {
                collection.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);
                builder.AddFile(file);
            });
        });

        if (!customHttpClient)
        {
            await http.SendAsync(server.Uri);
        }
        else
        {
            await http.SendAsync(new HttpOptions(new HttpRequestMessage(HttpMethod.Get, server.Uri))
            {
                ThrowOnNonSuccessStatusCode = false,
                HttpClient = new HttpClient()
            });
        }

        await host.DisposeAsync();

        var logFile = await File.ReadAllTextAsync(file);
        await Assert.That(logFile).Contains("HTTP Request:");
        await Assert.That(logFile).Contains($"GET {server.Uri} HTTP/1.1");
        await Assert.That(logFile).Contains("HTTP Response:");
        await Assert.That(logFile).Contains("Headers");
        await Assert.That(logFile).Contains("Server: LocalHttpServer");
        await Assert.That(logFile).Contains("Body");
        await Assert.That(logFile).Contains("Duration:");
        await Assert.That(logFile).Contains("HTTP Status:");

        var logFileLines = (await File.ReadAllLinesAsync(file)).ToList();

        var indexOfRequest = logFileLines.FindIndex(x => x.Contains("HTTP Request:"));
        var indexOfStatusCode = logFileLines.FindIndex(x => x.Contains("HTTP Status:"));
        var indexOfDuration = logFileLines.FindIndex(x => x.Contains("Duration:"));
        var indexOfResponse = logFileLines.FindIndex(x => x.Contains("HTTP Response:"));

        using (Assert.Multiple())
        {
            await Assert.That(indexOfRequest).IsLessThan(indexOfStatusCode);
            await Assert.That(indexOfStatusCode).IsLessThan(indexOfDuration);
            await Assert.That(indexOfDuration).IsLessThan(indexOfResponse);
        }
    }
}
