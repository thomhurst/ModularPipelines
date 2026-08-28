using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Distributed.Extensions;
using ModularPipelines.Distributed.Redis.Configuration;
using ModularPipelines.Distributed.Redis.Extensions;

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
            await Assert.That(distributedOptions.ExecutionIdentifier).IsEqualTo("current-run");
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
}
