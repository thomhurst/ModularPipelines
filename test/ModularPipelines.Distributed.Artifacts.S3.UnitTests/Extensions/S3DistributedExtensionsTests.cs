using ModularPipelines.Context;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Artifacts.S3.Extensions;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.Artifacts.S3.UnitTests.Extensions;

[TUnit.Core.NotInParallel("ProcessEnvironment")]
public class S3DistributedExtensionsTests
{
    private static readonly string[] ExecutionEnvironmentVariables =
    [
        "GITHUB_RUN_ID",
        "GITHUB_RUN_ATTEMPT",
        "MODULARPIPELINES_RUN_ID",
        "BUILD_BUILDID",
        "CI_PIPELINE_ID",
    ];

    [Test]
    public async Task ArtifactStore_Rejects_Unconfigured_RunId()
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
            builder.AddS3DistributedArtifactStore(options =>
                options.BucketName = "artifact-only");

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => builder.BuildAsync());

            await Assert.That(exception!.Message).Contains(nameof(DistributedOptions.RunId));
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
