using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ModularPipelines.Distributed.Discovery.Redis.UnitTests;

public class ImmutableOptionsTests
{
    [Test]
    public async Task Binding_And_Transforms_Apply_In_Registration_Order()
    {
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Discovery:ConnectionString"] = "localhost:6379",
            ["Discovery:KeyPrefix"] = "bound",
        }).Build();
        var builder = Pipeline.CreateBuilder();
        builder.AddRedisMasterDiscovery(options => options with { ConnectionString = "first" });
        builder.AddRedisMasterDiscovery(configuration.GetSection("Discovery"));
        builder.AddRedisMasterDiscovery(options => options with { KeyPrefix = options.KeyPrefix + "-last" });
        using var provider = builder.Services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<RedisDiscoveryOptions>>().Value;
        await Assert.That(options.ConnectionString).IsEqualTo("localhost:6379");
        await Assert.That(options.KeyPrefix).IsEqualTo("bound-last");
    }

    [Test]
    public async Task Null_Transform_Result_Is_Rejected()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddRedisMasterDiscovery(_ => null!);
        using var provider = builder.Services.BuildServiceProvider();
        await Assert.That(() => provider.GetRequiredService<IOptions<RedisDiscoveryOptions>>().Value)
            .Throws<InvalidOperationException>().WithMessageContaining("RedisDiscoveryOptions configuration returned null");
    }

    [Test]
    public async Task Record_Copy_Does_Not_Reuse_Previous_Connection_Snapshot()
    {
        var original = new RedisDiscoveryOptions { ConnectionString = "localhost:6379" };
        var first = original.GetConnectionConfiguration();
        var updated = original with { ConnectionString = "localhost:6380" };
        await Assert.That(first.EndPoints.Single().ToString()).Contains("6379");
        await Assert.That(updated.GetConnectionConfiguration().EndPoints.Single().ToString()).Contains("6380");
        await Assert.That(original.GetConnectionConfiguration().EndPoints.Single().ToString()).Contains("6379");
    }
}
