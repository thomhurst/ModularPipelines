using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Caching;
using ModularPipelines.Context;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Artifacts.S3;
using ModularPipelines.Distributed.Artifacts.S3.Caching;
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
            builder.AddS3DistributedArtifactStore(options => options with { BucketName = "artifact-only" });

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
        builder.ConfigureDistributedOptions(options => options with { RunId = "bucket-run" });
        builder.AddS3DistributedArtifactStore(_ => _);

        await Assert.That(async () => await builder.BuildAsync())
            .Throws<OptionsValidationException>()
            .WithMessageContaining(nameof(S3StorageOptions.BucketName));
    }

    [Test]
    public async Task Part_Size_Below_S3_Minimum_Is_Rejected()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<NoOpModule>();
        builder.AddS3ModuleCache(options => options with
        {
            BucketName = "cache",
            MultipartPartSizeBytes = 1024,
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

        await Assert.That(() => builder.AddS3DistributedArtifactStore(options => options with { BucketName = "bucket" }))
            .Throws<InvalidOperationException>()
            .WithMessageContaining(nameof(OtherArtifactStoreFactory));
    }

    [Test]
    [Arguments(false, "default")]
    [Arguments(false, "null")]
    [Arguments(false, "transform")]
    [Arguments(true, "default")]
    [Arguments(true, "null")]
    [Arguments(true, "transform")]
    public async Task ModuleCache_Overloads_Preserve_Registration_And_Cache_Options(
        bool useConfigurationSection,
        string cacheConfiguration)
    {
        var builder = Pipeline.CreateBuilder();
        var section = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["S3:BucketName"] = "cache-bucket",
                ["S3:KeyPrefix"] = "cache-only",
            })
            .Build()
            .GetSection("S3");
        S3StorageOptions ConfigureS3(S3StorageOptions options) => options with
        {
            BucketName = "cache-bucket",
            KeyPrefix = "cache-only",
        };
        ModuleCacheOptions ConfigureCache(ModuleCacheOptions options) => options with { MaxInputFiles = 42 };

        var returnedBuilder = (useConfigurationSection, cacheConfiguration) switch
        {
            (false, "default") => builder.AddS3ModuleCache(ConfigureS3),
            (false, "null") => builder.AddS3ModuleCache(ConfigureS3, null),
            (false, "transform") => builder.AddS3ModuleCache(ConfigureS3, ConfigureCache),
            (true, "default") => builder.AddS3ModuleCache(section),
            (true, "null") => builder.AddS3ModuleCache(section, null),
            (true, "transform") => builder.AddS3ModuleCache(section, ConfigureCache),
            _ => throw new ArgumentOutOfRangeException(nameof(cacheConfiguration)),
        };

        using var serviceProvider = builder.Services.BuildServiceProvider();
        var s3Options = serviceProvider.GetRequiredService<IOptionsMonitor<S3StorageOptions>>()
            .Get("ModularPipelines.S3ModuleCache");
        var cacheOptions = serviceProvider.GetRequiredService<IOptions<ModuleCacheOptions>>().Value;

        using (Assert.Multiple())
        {
            await Assert.That(returnedBuilder).IsSameReferenceAs(builder);
            await Assert.That(s3Options.BucketName).IsEqualTo("cache-bucket");
            await Assert.That(s3Options.KeyPrefix).IsEqualTo("cache-only");
            await Assert.That(cacheOptions.MaxInputFiles)
                .IsEqualTo(cacheConfiguration == "transform" ? 42 : new ModuleCacheOptions().MaxInputFiles);
            await Assert.That(serviceProvider.GetRequiredService<IModuleCacheStore>()).IsTypeOf<S3ModuleCache>();
            await Assert.That(builder.Services.Any(descriptor =>
                descriptor.ServiceType == typeof(IDistributedCoordinatorFactory))).IsFalse();
        }
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
