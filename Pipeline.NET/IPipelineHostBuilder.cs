using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Pipeline.NET;

public interface IPipelineHostBuilder
{
    static IPipelineHostBuilder Create() => new PipelineHostBuilder();
    PipelineHostBuilder AddModule<TModule>() where TModule : class, IModule;
    PipelineHostBuilder AddModulesFromAssemblyContainingType<T>();
    PipelineHostBuilder AddModulesFromAssembly(Assembly assembly);
    IPipelineHostBuilder ConfigureAppConfiguration(Action<IConfigurationBuilder> configureDelegate);
    IPipelineHostBuilder ConfigureServices(Action<IServiceCollection> configureDelegate);
    Task<IModule[]> ExecutePipelineAsync();
}