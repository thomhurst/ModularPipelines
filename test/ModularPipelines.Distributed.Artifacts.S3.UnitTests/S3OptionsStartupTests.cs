using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.Artifacts.S3.UnitTests;

public class S3OptionsStartupTests
{
    private const string CacheOptionsName = "ModularPipelines.S3ModuleCache";

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Standalone_Cache_Starts_With_Only_Named_Options(bool bindConfiguration)
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<NoOpModule>();
        AddCache(builder, "cache-bucket", bindConfiguration);

        await using var pipeline = await builder.BuildAsync();
        var options = pipeline.Services.GetRequiredService<IOptionsMonitor<S3StorageOptions>>().Get(CacheOptionsName);

        await Assert.That(options.BucketName).IsEqualTo("cache-bucket");
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Artifact_And_Cache_Options_Remain_Isolated(bool cacheFirst)
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<NoOpModule>();
        builder.ConfigureDistributedOptions(options => options with { RunId = "s3-options-test" });
        if (cacheFirst)
        {
            AddCache(builder, "cache-bucket", false);
        }

        builder.AddS3DistributedArtifactStore(options => options with { BucketName = "artifact-bucket" });
        if (!cacheFirst)
        {
            AddCache(builder, "cache-bucket", false);
        }

        await using var pipeline = await builder.BuildAsync();
        var options = pipeline.Services.GetRequiredService<IOptionsMonitor<S3StorageOptions>>();

        await Assert.That(options.CurrentValue.BucketName).IsEqualTo("artifact-bucket");
        await Assert.That(options.Get(CacheOptionsName).BucketName).IsEqualTo("cache-bucket");
    }

    [Test]
    [Arguments(false, false)]
    [Arguments(false, true)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task Invalid_Registered_Backend_Is_Rejected(bool invalidCache, bool bindConfiguration)
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<NoOpModule>();
        builder.ConfigureDistributedOptions(options => options with { RunId = "s3-options-test" });
        builder.AddS3DistributedArtifactStore(options => options with
        {
            BucketName = invalidCache ? "artifact-bucket" : "",
        });
        AddCache(builder, invalidCache ? "" : "cache-bucket", bindConfiguration);

        var exception = await Assert.ThrowsAsync<OptionsValidationException>(() => builder.BuildAsync());

        await Assert.That(exception!.OptionsName).IsEqualTo(invalidCache ? CacheOptionsName : "");
        await Assert.That(exception.Message).Contains(nameof(S3StorageOptions.BucketName));
    }

    private static void AddCache(PipelineBuilder builder, string bucket, bool bindConfiguration)
    {
        if (bindConfiguration)
        {
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["S3:BucketName"] = bucket,
            }).Build();
            builder.AddS3ModuleCache(configuration.GetSection("S3"));
        }
        else
        {
            builder.AddS3ModuleCache(options => options with { BucketName = bucket });
        }
    }

    private sealed class NoOpModule : Module<int>
    {
        protected override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult(0);
    }
}
