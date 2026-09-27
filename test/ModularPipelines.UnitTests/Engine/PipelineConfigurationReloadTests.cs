using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.Engine;

public class PipelineConfigurationReloadTests
{
    [Test]
    public async Task AppSettings_Are_Not_Watched_For_Reloads_By_Default()
    {
        var sources = await GetJsonSourcesAsync([]);

        await Assert.That(sources).IsNotEmpty();
        await Assert.That(sources.Select(source => source.ReloadOnChange)).DoesNotContain(true);
    }

    [Test]
    public async Task Explicit_Host_Setting_Still_Enables_Reloads()
    {
        var sources = await GetJsonSourcesAsync(["--hostBuilder:reloadConfigOnChange", "true"]);

        await Assert.That(sources).IsNotEmpty();
        await Assert.That(sources.Select(source => source.ReloadOnChange)).DoesNotContain(false);
    }

    private static async Task<FileConfigurationSource[]> GetJsonSourcesAsync(string[] args)
    {
        var builder = Pipeline.CreateBuilder(args);
        builder.AddModule<NoOpModule>();
        await using var pipeline = await builder.BuildAsync();
        var configuration = (IConfigurationRoot) pipeline.Services.GetRequiredService<IConfiguration>();
        return
        [
            .. configuration.Providers
                .OfType<JsonConfigurationProvider>()
                .Select(provider => provider.Source),
        ];
    }

    private sealed class NoOpModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult(true);
    }
}
