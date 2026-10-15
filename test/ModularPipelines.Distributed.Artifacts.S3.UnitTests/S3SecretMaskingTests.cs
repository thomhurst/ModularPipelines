using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.Artifacts.S3.UnitTests;

public class S3SecretMaskingTests
{
    private const string CacheAccessKey = "cache-private-access-key";
    private const string CacheSecretKey = "cache-private-secret-key";
    private const string ArtifactAccessKey = "artifact-private-access-key";
    private const string ArtifactSecretKey = "artifact-private-secret-key";

    [Test]
    [Arguments(false, false)]
    [Arguments(false, true)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task Startup_Masks_Credentials_From_Every_Registered_Backend(bool bindConfiguration, bool includeArtifacts)
    {
        var logger = new CapturingLogger();
        var builder = Pipeline.CreateBuilder();
        builder.ConfigureOptions(options => options with
        {
            Console = options.Console with { ShowProgress = false, PrintLogo = false, PrintResults = false },
            RunReport = options.RunReport with { AutoWriteInCi = false, HistoryRetention = 0 },
        });
        builder.AddModule<CredentialLoggingModule>();
        builder.Services.AddSingleton<ILogger<CredentialLoggingModule>>(logger);
        if (bindConfiguration)
        {
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Cache:BucketName"] = "cache-bucket",
                ["Cache:AccessKey"] = CacheAccessKey,
                ["Cache:SecretKey"] = CacheSecretKey,
            }).Build();
            builder.AddS3ModuleCache(configuration.GetSection("Cache"));
        }
        else
        {
            builder.AddS3ModuleCache(options => options with
            {
                BucketName = "cache-bucket",
                AccessKey = CacheAccessKey,
                SecretKey = CacheSecretKey,
            });
        }

        if (includeArtifacts)
        {
            builder.ConfigureDistributedOptions(options => options with { RunId = "s3-secret-test" });
            builder.AddS3DistributedArtifactStore(options => options with
            {
                BucketName = "artifact-bucket",
                AccessKey = ArtifactAccessKey,
                SecretKey = ArtifactSecretKey,
            });
        }

        await using (var pipeline = await builder.BuildAsync())
        {
            await pipeline.RunAsync();
        }

        var output = logger.Output.ToString();
        await Assert.That(output).Contains("public-marker");
        await Assert.That(output).Contains("**********");
        await Assert.That(output).DoesNotContain(CacheAccessKey);
        await Assert.That(output).DoesNotContain(CacheSecretKey);
        if (includeArtifacts)
        {
            await Assert.That(output).DoesNotContain(ArtifactAccessKey);
            await Assert.That(output).DoesNotContain(ArtifactSecretKey);
        }
    }

    private sealed class CredentialLoggingModule : Module<int>
    {
        protected override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            if (context.Logger.IsEnabled(LogLevel.Information))
            {
                context.Logger.LogInformation("public-marker {CacheAccess} {CacheSecret} {ArtifactAccess} {ArtifactSecret}",
                    CacheAccessKey, CacheSecretKey, ArtifactAccessKey, ArtifactSecretKey);
            }

            return Task.FromResult(0);
        }
    }

    private sealed class CapturingLogger : ILogger<CredentialLoggingModule>
    {
        public StringBuilder Output { get; } = new();

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
            Func<TState, Exception?, string> formatter) => Output.AppendLine(formatter(state, exception));

        public bool IsEnabled(LogLevel logLevel) => true;

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
    }
}
