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
                options.RunIdentifier = "artifact-options-test";
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
    public async Task CoordinatorRunIdentifierScopesWorkerRegistrations()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddDistributedMode(_ => { });
        builder.AddRedisDistributedCoordinator(options =>
        {
            options.ConnectionString = "unused";
            options.RunIdentifier = "current-run";
        });
        using var serviceProvider = builder.Services.BuildServiceProvider();

        var distributedOptions = serviceProvider
            .GetRequiredService<IOptions<DistributedOptions>>()
            .Value;
        var redisOptions = serviceProvider.GetRequiredService<RedisDistributedOptions>();

        using (Assert.Multiple())
        {
            await Assert.That(distributedOptions.RunIdentifier).IsEqualTo("current-run");
            await Assert.That(redisOptions.RunIdentifier).IsEqualTo("current-run");
        }
    }

    [Test]
    public async Task CoordinatorRequiresInvocationScopedRunIdentifier()
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
            builder.AddDistributedMode(_ => { });

            var exception = Assert.Throws<InvalidOperationException>(() =>
                builder.AddRedisDistributedCoordinator(options =>
                    options.ConnectionString = "unused"));

            await Assert.That(exception.Message).Contains("unique RunIdentifier");
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
