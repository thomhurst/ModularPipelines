using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Host;

public interface IPipelineHostBuilder
{
    static IPipelineHostBuilder Create() => new PipelineHostBuilder();
    IPipelineHostBuilder ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate);
    IPipelineHostBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate);
    IPipelineHostBuilder ConfigurePipelineOptions(Action<HostBuilderContext, PipelineOptions> configureDelegate);
    Task<IReadOnlyList<ModuleBase>> ExecutePipelineAsync();
    internal IHost BuildHost();
}
