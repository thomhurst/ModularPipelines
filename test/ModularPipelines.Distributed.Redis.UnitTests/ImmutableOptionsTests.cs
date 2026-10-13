using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ModularPipelines.Distributed.Redis.UnitTests;

public class ImmutableOptionsTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task String_Output_Redacts_Credentials_Including_Record_Copies(bool copy)
    {
        var options = new RedisOptions
        {
            ConnectionString = "localhost:6379,user=private-user,password=private-password",
            KeyPrefix = "diagnostic-prefix",
        };
        if (copy)
        {
            options = options with { ChunkSizeBytes = 12345 };
        }

        var output = $"{options}";
        await Assert.That(output).DoesNotContain(options.ConnectionString);
        await Assert.That(output).DoesNotContain("private-user");
        await Assert.That(output).DoesNotContain("private-password");
        await Assert.That(output).Contains("ConnectionString = <redacted>");
        await Assert.That(output).Contains("KeyPrefix = diagnostic-prefix");
    }

    [Test]
    public async Task Binding_And_Transforms_Apply_In_Registration_Order()
    {
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Redis:ConnectionString"] = "localhost:6379",
            ["Redis:KeyPrefix"] = "bound",
        }).Build();
        var builder = Pipeline.CreateBuilder();
        builder.AddRedisDistributed(options => options with { ConnectionString = "first" });
        builder.AddRedisDistributed(configuration.GetSection("Redis"));
        builder.AddRedisDistributed(options => options with { KeyPrefix = options.KeyPrefix + "-last" });
        using var provider = builder.Services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<RedisOptions>>().Value;
        await Assert.That(options.ConnectionString).IsEqualTo("localhost:6379");
        await Assert.That(options.KeyPrefix).IsEqualTo("bound-last");
    }

    [Test]
    public async Task Null_Transform_Result_Is_Rejected()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddRedisDistributed(_ => null!);
        using var provider = builder.Services.BuildServiceProvider();
        await Assert.That(() => provider.GetRequiredService<IOptions<RedisOptions>>().Value)
            .Throws<InvalidOperationException>().WithMessageContaining("RedisOptions configuration returned null");
    }

    [Test]
    public async Task Named_Cache_Binding_And_Both_Transforms_Are_Preserved()
    {
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Cache:ConnectionString"] = "localhost:6379",
            ["Cache:KeyPrefix"] = "bound-cache",
        }).Build();
        var builder = Pipeline.CreateBuilder();
        builder.AddRedisModuleCache(configuration.GetSection("Cache"));
        builder.AddRedisModuleCache(options => options with { KeyPrefix = options.KeyPrefix + "-last" },
            cache => cache with { MaxCacheEntryBytes = 12345 });
        using var provider = builder.Services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptionsMonitor<RedisOptions>>().Get("ModularPipelines.RedisModuleCache");
        await Assert.That(options.ConnectionString).IsEqualTo("localhost:6379");
        await Assert.That(options.KeyPrefix).IsEqualTo("bound-cache-last");
        await Assert.That(provider.GetRequiredService<IOptions<ModularPipelines.Caching.ModuleCacheOptions>>().Value.MaxCacheEntryBytes)
            .IsEqualTo(12345);
        await Assert.That(provider.GetRequiredService<IOptions<RedisOptions>>().Value.ConnectionString).IsEqualTo(string.Empty);
    }
}
