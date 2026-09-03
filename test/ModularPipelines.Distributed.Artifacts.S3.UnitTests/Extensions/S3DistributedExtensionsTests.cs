using ModularPipelines.Context;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Artifacts.S3.Extensions;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.Artifacts.S3.UnitTests.Extensions;

[TUnit.Core.NotInParallel("ProcessEnvironment")]
public class S3DistributedExtensionsTests
{
    [Test]
    public async Task ArtifactStore_Rejects_Unconfigured_RunId()
    {
        var original = Environment.GetEnvironmentVariable("MODULARPIPELINES_RUN_ID");
        try
        {
            Environment.SetEnvironmentVariable("MODULARPIPELINES_RUN_ID", null);
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
            Environment.SetEnvironmentVariable("MODULARPIPELINES_RUN_ID", original);
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
