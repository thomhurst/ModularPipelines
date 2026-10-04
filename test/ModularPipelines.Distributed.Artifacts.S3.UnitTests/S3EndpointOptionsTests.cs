using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Distributed.Artifacts.S3.Artifacts;

namespace ModularPipelines.Distributed.Artifacts.S3.UnitTests;

public class S3EndpointOptionsTests
{
    [Test]
    [Arguments("relative/path", false)]
    [Arguments("ftp://storage.example", false)]
    [Arguments("file:///storage", false)]
    [Arguments("relative/path", true)]
    [Arguments("ftp://storage.example", true)]
    public async Task Configuration_Rejects_Non_Http_Endpoints(string endpoint, bool moduleCache)
    {
        await Assert.That(() => BindOptions(endpoint, moduleCache))
            .Throws<OptionsValidationException>().WithMessageContaining(nameof(S3StorageOptions.ServiceUrl));
    }

    [Test]
    [Arguments("http://localhost:9000", false)]
    [Arguments("https://storage.example/base%20path", false)]
    [Arguments("https://storage.example", true)]
    public async Task Bound_Uri_Reaches_Aws_Client(string endpoint, bool moduleCache)
    {
        var options = BindOptions(endpoint, moduleCache);
        using var client = S3ClientFactory.Create(options);

        await Assert.That(options.ServiceUrl).IsEqualTo(new Uri(endpoint));
        await Assert.That(new Uri(client.Config.ServiceURL)).IsEqualTo(new Uri(endpoint));
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Omitted_Endpoint_Retains_Aws_Region_Default(bool moduleCache)
    {
        var options = BindOptions(null, moduleCache);
        using var client = S3ClientFactory.Create(options);

        await Assert.That(options.ServiceUrl).IsNull();
        await Assert.That(client.Config.ServiceURL).IsNull();
        await Assert.That(client.Config.RegionEndpoint.SystemName).IsEqualTo("us-east-1");
    }

    private static S3StorageOptions BindOptions(string? endpoint, bool moduleCache)
    {
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["S3:BucketName"] = "bucket",
            ["S3:ServiceUrl"] = endpoint,
            ["S3:AccessKey"] = "test-access-key",
            ["S3:SecretKey"] = "test-secret-key",
        }).Build();
        var builder = Pipeline.CreateBuilder();
        if (moduleCache)
        {
            builder.AddS3ModuleCache(configuration.GetSection("S3"));
        }
        else
        {
            builder.AddS3DistributedArtifactStore(configuration.GetSection("S3"));
        }

        using var services = builder.Services.BuildServiceProvider();
        var options = services.GetRequiredService<IOptionsMonitor<S3StorageOptions>>();
        return options.Get(moduleCache ? "ModularPipelines.S3ModuleCache" : Microsoft.Extensions.Options.Options.DefaultName);
    }
}
