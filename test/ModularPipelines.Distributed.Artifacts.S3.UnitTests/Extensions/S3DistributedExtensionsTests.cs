using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Context;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Artifacts.S3;
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

    [Test]
    public async Task ArtifactStore_Rejects_Missing_BucketName_At_Startup()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<NoOpModule>();
        builder.Services.Configure<DistributedOptions>(options => options.RunId = "bucket-run");
        builder.AddS3DistributedArtifactStore(_ => { });

        await Assert.That(async () => await builder.BuildAsync())
            .Throws<OptionsValidationException>()
            .WithMessageContaining(nameof(S3StorageOptions.BucketName));
    }

    [Test]
    public async Task Part_Size_Below_S3_Minimum_Is_Rejected()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<NoOpModule>();
        builder.AddS3ModuleCache(options =>
        {
            options.BucketName = "cache";
            options.MultipartPartSizeBytes = 1024;
        });

        await Assert.That(async () => await builder.BuildAsync())
            .Throws<OptionsValidationException>()
            .WithMessageContaining(nameof(S3StorageOptions.MultipartPartSizeBytes));
    }

    [Test]
    public async Task A_Second_Artifact_Store_Backend_Is_Rejected()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddDistributedArtifactStoreFactory<OtherArtifactStoreFactory>();

        await Assert.That(() => builder.AddS3DistributedArtifactStore(options => options.BucketName = "bucket"))
            .Throws<InvalidOperationException>()
            .WithMessageContaining(nameof(OtherArtifactStoreFactory));
    }

    private sealed class OtherArtifactStoreFactory : IDistributedArtifactStoreFactory
    {
        public Task<IDistributedArtifactStore> CreateAsync(CancellationToken cancellationToken) =>
            throw new NotSupportedException();
    }

    private sealed class NoOpModule : Module<int>
    {
        protected override Task<int> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult(0);
    }
}
