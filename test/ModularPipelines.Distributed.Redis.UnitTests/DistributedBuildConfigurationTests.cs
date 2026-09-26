using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Build.Helpers;
using StackExchange.Redis;

namespace ModularPipelines.Distributed.Redis.UnitTests;

public class DistributedBuildConfigurationTests
{
    [Test]
    [Arguments(null, null)]
    [Arguments("0", "1")]
    public async Task Local_And_Single_Instance_Builds_Do_Not_Register_Redis(string? index, string? count)
    {
        var builder = Pipeline.CreateBuilder([]);
        var environment = CreateEnvironment(index, count);

        await Assert.That(DistributedBuildConfiguration.Configure(builder, environment.GetValueOrDefault)).IsTrue();
        await Assert.That(builder.Services.Any(service => service.ServiceType == typeof(IConnectionMultiplexer))).IsFalse();
    }

    [Test]
    [Arguments("0", true)]
    [Arguments("1", false)]
    [Arguments("3", false)]
    public async Task Missing_Secrets_Run_Only_The_Standalone_Primary(string index, bool shouldRun)
    {
        foreach (var missing in new[] { "REDIS_ENDPOINT", "REDIS_KEY" })
        {
            var builder = Pipeline.CreateBuilder([]);
            var environment = CreateEnvironment(index, "4");
            environment[missing] = null;

            await Assert.That(DistributedBuildConfiguration.Configure(builder, environment.GetValueOrDefault)).IsEqualTo(shouldRun);
            await Assert.That(builder.Services.Any(service => service.ServiceType == typeof(IConnectionMultiplexer))).IsFalse();
        }
    }

    [Test]
    [Arguments("0")]
    [Arguments("1")]
    public async Task Configures_One_Attempt_Scoped_Run_With_Master_Only_Capability(string index)
    {
        var builder = Pipeline.CreateBuilder([]);
        var environment = CreateEnvironment(index, "4");
        await Assert.That(DistributedBuildConfiguration.Configure(builder, environment.GetValueOrDefault)).IsTrue();
        await using var services = builder.Services.BuildServiceProvider();
        var distributed = services.GetRequiredService<IOptions<DistributedOptions>>().Value;
        var redis = services.GetRequiredService<IOptions<RedisDistributedOptions>>().Value;
        var artifacts = services.GetRequiredService<IOptions<ArtifactOptions>>().Value;
        var connection = services.GetRequiredService<ConfigurationOptions>();

        await Assert.That(distributed.RunId).IsEqualTo("123-2");
        await Assert.That(distributed.TotalInstances).IsEqualTo(4);
        await Assert.That(distributed.InstanceIndex).IsEqualTo(int.Parse(index));
        await Assert.That(distributed.MinimumWorkerCount).IsEqualTo(3);
        await Assert.That(distributed.Capabilities.Contains(new Capability("ci-master"))).IsEqualTo(index == "0");
        await Assert.That(distributed.ModuleResultTimeout < TimeSpan.FromMinutes(90)).IsTrue();
        await Assert.That(redis.KeyExpiration > distributed.ModuleResultTimeout).IsTrue();
        await Assert.That(artifacts.TimeToLive > distributed.ModuleResultTimeout).IsTrue();
        await Assert.That(connection.Ssl).IsTrue();
        await Assert.That(connection.AbortOnConnectFail).IsFalse();
        await Assert.That(connection.Password).IsEqualTo("key,with=special;characters");
    }

    [Test]
    public async Task Explicit_Run_Id_Is_Preserved_Without_GitHub_Environment()
    {
        var builder = Pipeline.CreateBuilder([]);
        var environment = CreateEnvironment("0", "2");
        environment["MODULARPIPELINES_RUN_ID"] = "explicit-attempt";
        environment.Remove("GITHUB_RUN_ID");
        environment.Remove("GITHUB_RUN_ATTEMPT");
        DistributedBuildConfiguration.Configure(builder, environment.GetValueOrDefault);
        await using var services = builder.Services.BuildServiceProvider();

        await Assert.That(services.GetRequiredService<IOptions<DistributedOptions>>().Value.RunId).IsEqualTo("explicit-attempt");
    }

    [Test]
    [Arguments(null, "4")]
    [Arguments("0", null)]
    [Arguments("-1", "4")]
    [Arguments("4", "4")]
    [Arguments("0", "0")]
    public async Task Invalid_Matrix_Identity_Is_Rejected(string? index, string? count)
    {
        var builder = Pipeline.CreateBuilder([]);
        var environment = CreateEnvironment(index, count);

        await Assert.That(() => DistributedBuildConfiguration.Configure(builder, environment.GetValueOrDefault))
            .Throws<InvalidOperationException>();
    }

    [Test]
    public async Task Distributed_Build_Without_An_Attempt_Identity_Is_Rejected()
    {
        var builder = Pipeline.CreateBuilder([]);
        var environment = CreateEnvironment("0", "4");
        environment.Remove("GITHUB_RUN_ATTEMPT");

        await Assert.That(() => DistributedBuildConfiguration.Configure(builder, environment.GetValueOrDefault))
            .Throws<InvalidOperationException>();
    }

    private static Dictionary<string, string?> CreateEnvironment(string? index, string? count) => new()
    {
        ["MODULARPIPELINES_INSTANCE_INDEX"] = index,
        ["MODULARPIPELINES_TOTAL_INSTANCES"] = count,
        ["REDIS_ENDPOINT"] = "redis.example.test:6380",
        ["REDIS_KEY"] = "key,with=special;characters",
        ["GITHUB_RUN_ID"] = "123",
        ["GITHUB_RUN_ATTEMPT"] = "2",
    };
}
