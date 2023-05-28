using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pipeline.NET.Modules;
using Pipeline.NET.Options;

namespace Pipeline.NET.Host;

public interface IPipelineHostBuilder
{
    static IPipelineHostBuilder Create() => new PipelineHostBuilder();
    PipelineHostBuilder AddModule<TModule>() where TModule : class, IModule;
    PipelineHostBuilder AddModulesFromAssemblyContainingType<T>();
    PipelineHostBuilder AddModulesFromAssembly(Assembly assembly);
    IPipelineHostBuilder ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate);
    IPipelineHostBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate);
    IPipelineHostBuilder ConfigurePipelineOptions(Action<HostBuilderContext, PipelineOptions> configureDelegate);
    Task<IModule[]> ExecutePipelineAsync();
}