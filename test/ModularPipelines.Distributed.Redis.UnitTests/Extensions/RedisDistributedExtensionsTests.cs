using System.IO.Compression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Context;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Extensions;
using ModularPipelines.Distributed.Redis.Configuration;
using ModularPipelines.Distributed.Redis.Extensions;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;
using StackExchange.Redis;

namespace ModularPipelines.Distributed.Redis.UnitTests.Extensions;

[TUnit.Core.NotInParallel("ProcessEnvironment")]
public class RedisDistributedExtensionsTests
{
    private static readonly string[] ExecutionEnvironmentVariables =
    [
        "GITHUB_RUN_ID",
        "GITHUB_RUN_ATTEMPT",
        "RUN_IDENTIFIER",
        "BUILD_BUILDID",
        "CI_PIPELINE_ID",
    ];

    [Test]
    public async Task ArtifactOptionsUseSharedOptionsPipeline()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<NoOpModule>();
        builder.AddRedisDistributed(
            options =>
            {
                options.ConnectionString = "unused";
            },
            options => options.CompressionLevel = CompressionLevel.NoCompression);
        await using var pipeline = await builder.BuildAsync();

        var configuredOptions = pipeline.Services.GetRequiredService<IOptions<ArtifactOptions>>().Value;
        var directOptions = pipeline.Services.GetRequiredService<ArtifactOptions>();

        using (Assert.Multiple())
        {
            await Assert.That(directOptions).IsSameReferenceAs(configuredOptions);
            await Assert.That(configuredOptions.CompressionLevel)
                .IsEqualTo(CompressionLevel.NoCompression);
        }
    }

    [Test]
    public async Task ArtifactStoreRegistersStandaloneRedisDependencies()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<NoOpModule>();

        builder.AddRedisDistributedArtifactStore(options =>
            options.ConnectionString = "artifact-only");

        await using var pipeline = await builder.BuildAsync();
        var redisOptions = pipeline.Services.GetRequiredService<RedisDistributedOptions>();
        var distributedOptions = pipeline.Services.GetRequiredService<IOptions<DistributedOptions>>().Value;

        using (Assert.Multiple())
        {
            await Assert.That(redisOptions.ConnectionString).IsEqualTo("artifact-only");
            await Assert.That(distributedOptions.RunId).IsNotEmpty();
            await Assert.That(builder.Services.Any(descriptor =>
                descriptor.ServiceType == typeof(IConnectionMultiplexer))).IsTrue();
            await Assert.That(builder.Services.Any(descriptor =>
                descriptor.ServiceType == typeof(IDistributedArtifactStoreFactory))).IsTrue();
        }
    }

    [Test]
    public async Task Coordinator_Uses_Core_RunId()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<NoOpModule>();
        builder.AddDistributedMode(options => options.RunId = "current-run");
        builder.AddRedisDistributedCoordinator(options =>
        {
            options.ConnectionString = "unused";
        });
        await using var pipeline = await builder.BuildAsync();

        var distributedOptions = pipeline.Services
            .GetRequiredService<IOptions<DistributedOptions>>()
            .Value;
        await Assert.That(distributedOptions.RunId).IsEqualTo("current-run");
        await Assert.That(typeof(RedisDistributedOptions).GetProperty("RunIdentifier")).IsNull();
    }

    [Test]
    public async Task Distributed_Mode_Generates_RunId_When_Unconfigured()
    {
        var originals = ExecutionEnvironmentVariables.ToDictionary(
            name => name,
            Environment.GetEnvironmentVariable);

        try
        {
            foreach (var name in ExecutionEnvironmentVariables)
            {
                Environment.SetEnvironmentVariable(name, null);
            }

            var builder = Pipeline.CreateBuilder();
            builder.AddModule<NoOpModule>();
            builder.AddDistributedMode(_ => { });

            builder.AddRedisDistributedCoordinator(options =>
                options.ConnectionString = "unused");
            await using var pipeline = await builder.BuildAsync();
            var options = pipeline.Services.GetRequiredService<IOptions<DistributedOptions>>().Value;

            await Assert.That(Guid.TryParseExact(options.RunId, "N", out _)).IsTrue();
        }
        finally
        {
            foreach (var (name, value) in originals)
            {
                Environment.SetEnvironmentVariable(name, value);
            }
        }
    }

    [Test]
    public async Task Distributed_Mode_Rejects_Unconfigured_Multi_Instance_Run()
    {
        var originals = ExecutionEnvironmentVariables.ToDictionary(
            name => name,
            Environment.GetEnvironmentVariable);

        try
        {
            foreach (var name in ExecutionEnvironmentVariables)
            {
                Environment.SetEnvironmentVariable(name, null);
            }

            var builder = Pipeline.CreateBuilder();
            builder.AddDistributedMode(options => options.TotalInstances = 2);
            builder.AddRedisDistributedCoordinator(options =>
                options.ConnectionString = "unused");

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => builder.BuildAsync());

            using (Assert.Multiple())
            {
                await Assert.That(exception!.Message).Contains(nameof(DistributedOptions.RunId));
                await Assert.That(exception.Message).Contains("RUN_IDENTIFIER");
            }
        }
        finally
        {
            foreach (var (name, value) in originals)
            {
                Environment.SetEnvironmentVariable(name, value);
            }
        }
    }

    private sealed class NoOpModule : Module<int>
    {
        protected override Task<int> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult(0);
    }
}
