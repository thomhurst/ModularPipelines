using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Http;
using ModularPipelines.Options;

namespace ModularPipelines.UnitTests;

public class HttpTests : TestBase
{
    [Test]
    public async Task When_Log_Request_False_Then_Do_Not_Log_Request()
    {
        var file = Path.Combine(Environment.CurrentDirectory, Guid.NewGuid().ToString("N") + ".txt");
        
        var result = await GetService<IHttp>((_, collection) =>
        {
            collection.AddLogging(builder =>
            {
                builder.AddFile(file);
            });
        });
        
        await result.T.SendAsync(new HttpOptions(new HttpRequestMessage(HttpMethod.Get, "https://www.github.com"))
        {
            LogRequest = false
        });

        await Dispose(result.Host.Services);
        result.Host.Dispose();

        var logFile = await File.ReadAllTextAsync(file);
        
        Assert.That(logFile, Does.Contain("INFO	[ModularPipelines.Http.Http]"));
        
        Assert.That(logFile, Does.Not.Contain("---Request---"));
        Assert.That(logFile, Does.Not.Contain("GET https://www.github.com/ HTTP/1.1"));
        Assert.That(logFile, Does.Contain("---Response---"));
        Assert.That(logFile, Does.Contain("Server: GitHub.com"));
    }
    
    [Test]
    public async Task When_Log_Response_False_Then_Do_Not_Log_Request()
    {
        var file = Path.Combine(Environment.CurrentDirectory, Guid.NewGuid().ToString("N") + ".txt");
        
        var result = await GetService<IHttp>((_, collection) =>
        {
            collection.AddLogging(builder =>
            {
                builder.AddFile(file);
            });
        });
        
        await result.T.SendAsync(new HttpOptions(new HttpRequestMessage(HttpMethod.Get, "https://www.github.com"))
        {
            LogResponse = false
        });
        
        await Dispose(result.Host.Services);
        result.Host.Dispose();

        var logFile = await File.ReadAllTextAsync(file);
        
        Assert.That(logFile, Does.Contain("INFO	[ModularPipelines.Http.Http]"));
        
        Assert.That(logFile, Does.Contain("---Request---"));
        Assert.That(logFile, Does.Contain("GET https://www.github.com/ HTTP/1.1"));
        Assert.That(logFile, Does.Not.Contain("---Response---"));
        Assert.That(logFile, Does.Not.Contain("Server: GitHub.com"));
    }
    
    [Test]
    public async Task Assert_LoggingHttpClient_Logs_As_Expected()
    {
        var file = Path.Combine(Environment.CurrentDirectory, Guid.NewGuid().ToString("N") + ".txt");
        
        var result = await GetService<IHttp>((_, collection) =>
        {
            collection.AddLogging(builder =>
            {
                builder.AddFile(file);
            });
        });
        
        var loggingClient = result.T.LoggingHttpClient;

        await loggingClient.GetAsync("https://www.github.com");
        
        await Dispose(result.Host.Services);
        result.Host.Dispose();

        var logFile = await File.ReadAllTextAsync(file);
        
        Assert.That(logFile, Does.Contain("INFO	[ModularPipelines.Http.LoggingHttpHandler]"));
        
        Assert.That(logFile, Does.Contain("---Request---"));
        Assert.That(logFile, Does.Contain("GET https://www.github.com/ HTTP/1.1"));
        Assert.That(logFile, Does.Contain("---Response---"));
        Assert.That(logFile, Does.Contain("Headers"));
        Assert.That(logFile, Does.Contain("Server: GitHub.com"));
        Assert.That(logFile, Does.Contain("Body"));
    }
}