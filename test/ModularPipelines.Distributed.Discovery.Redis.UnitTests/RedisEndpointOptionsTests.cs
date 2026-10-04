using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ModularPipelines.Distributed.Discovery.Redis.UnitTests;

public class RedisEndpointOptionsTests
{
    [Test]
    [Arguments("http://localhost:8079")]
    [Arguments("http://127.0.0.1:8079")]
    [Arguments("http://[::1]:8079")]
    [Arguments("https://redis.example/base%20path")]
    public async Task Bound_Rest_Uri_Selects_Http_Store(string endpoint)
    {
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Discovery:ConnectionString"] = string.Empty,
            ["Discovery:RestUrl"] = endpoint,
            ["Discovery:RestToken"] = "test-token",
        }).Build();
        var builder = Pipeline.CreateBuilder();
        builder.AddRedisMasterDiscovery(configuration.GetSection("Discovery"));
        await using var services = builder.Services.BuildServiceProvider();
        var options = services.GetRequiredService<IOptions<RedisDiscoveryOptions>>().Value;

        await Assert.That(options.RestUrl).IsEqualTo(new Uri(endpoint));
        await Assert.That(services.GetRequiredService<IRedisDiscoveryStore>().GetType()).IsEqualTo(typeof(RestRedisDiscoveryStore));
    }

    [Test]
    [Arguments(null)]
    [Arguments("")]
    [Arguments("   ")]
    public async Task Omitted_Rest_Uri_Retains_Tcp_Discovery(string? endpoint)
    {
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Discovery:ConnectionString"] = "localhost:6379",
            ["Discovery:RestUrl"] = endpoint,
        }).Build();
        var builder = Pipeline.CreateBuilder();
        builder.AddRedisMasterDiscovery(configuration.GetSection("Discovery"));
        await using var services = builder.Services.BuildServiceProvider();

        await Assert.That(services.GetRequiredService<IOptions<RedisDiscoveryOptions>>().Value.RestUrl).IsNull();
        await Assert.That(services.GetRequiredService<IRedisDiscoveryStore>().GetType()).IsEqualTo(typeof(StackExchangeRedisDiscoveryStore));
    }

    [Test]
    [Arguments("http://redis.example")]
    [Arguments("http://192.0.2.1:8079")]
    [Arguments("relative/path")]
    [Arguments("ftp://redis.example")]
    [Arguments("file:///redis")]
    public async Task Configuration_Rejects_Unsupported_Rest_Endpoints(string endpoint)
    {
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Discovery:RestUrl"] = endpoint,
            ["Discovery:RestToken"] = "test-token",
        }).Build();
        var builder = Pipeline.CreateBuilder();
        builder.AddRedisMasterDiscovery(configuration.GetSection("Discovery"));
        await using var services = builder.Services.BuildServiceProvider();

        await Assert.That(() => services.GetRequiredService<IOptions<RedisDiscoveryOptions>>().Value)
            .Throws<OptionsValidationException>().WithMessageContaining(nameof(RedisDiscoveryOptions.RestUrl));
    }
}
