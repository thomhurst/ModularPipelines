using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ModularPipelines.Distributed.Artifacts.S3.UnitTests;

public class ImmutableOptionsTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task String_Output_Redacts_Credentials_Including_Record_Copies(bool copy)
    {
        var options = new S3StorageOptions
        {
            AccessKey = "private-access-key",
            SecretKey = "private-secret-key",
            BucketName = "diagnostic-bucket",
        };
        if (copy)
        {
            options = options with { ForcePathStyle = true };
        }

        var output = $"{options}";
        await Assert.That(output).DoesNotContain(options.AccessKey!);
        await Assert.That(output).DoesNotContain(options.SecretKey!);
        await Assert.That(output).Contains("AccessKey = <redacted>");
        await Assert.That(output).Contains("SecretKey = <redacted>");
        await Assert.That(output).Contains("BucketName = diagnostic-bucket");
    }

    [Test]
    public async Task Binding_And_Transforms_Apply_In_Registration_Order()
    {
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["S3:BucketName"] = "artifacts",
            ["S3:KeyPrefix"] = "bound",
        }).Build();
        var builder = Pipeline.CreateBuilder();
        builder.AddS3DistributedArtifactStore(options => options with { BucketName = "first" });
        builder.AddS3DistributedArtifactStore(configuration.GetSection("S3"));
        builder.AddS3DistributedArtifactStore(options => options with { BucketName = options.BucketName + "-last" });
        using var provider = builder.Services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<S3StorageOptions>>().Value;
        await Assert.That(options.BucketName).IsEqualTo("artifacts-last");
    }

    [Test]
    public async Task Null_Transform_Result_Is_Rejected()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddS3DistributedArtifactStore(_ => null!);
        using var provider = builder.Services.BuildServiceProvider();
        await Assert.That(() => provider.GetRequiredService<IOptions<S3StorageOptions>>().Value)
            .Throws<InvalidOperationException>().WithMessageContaining("S3StorageOptions configuration returned null");
    }

    [Test]
    public async Task Named_Cache_Binding_And_Both_Transforms_Are_Preserved()
    {
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Cache:BucketName"] = "cache-bucket",
            ["Cache:KeyPrefix"] = "bound-cache",
        }).Build();
        var builder = Pipeline.CreateBuilder();
        builder.AddS3ModuleCache(configuration.GetSection("Cache"));
        builder.AddS3ModuleCache(options => options with { KeyPrefix = options.KeyPrefix + "-last" },
            cache => cache with { MaxCacheEntryBytes = 12345 });
        using var provider = builder.Services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptionsMonitor<S3StorageOptions>>().Get("ModularPipelines.S3ModuleCache");
        await Assert.That(options.BucketName).IsEqualTo("cache-bucket");
        await Assert.That(options.KeyPrefix).IsEqualTo("bound-cache-last");
        await Assert.That(provider.GetRequiredService<IOptions<ModularPipelines.Caching.ModuleCacheOptions>>().Value.MaxCacheEntryBytes)
            .IsEqualTo(12345);
        await Assert.That(() => provider.GetRequiredService<IOptions<S3StorageOptions>>().Value)
            .Throws<OptionsValidationException>().WithMessageContaining(nameof(S3StorageOptions.BucketName));
    }
}
