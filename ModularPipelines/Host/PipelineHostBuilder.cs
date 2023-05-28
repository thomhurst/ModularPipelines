using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Helpers;
using ModularPipelines.Interfaces;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.Requirements;
using TomLonghurst.Microsoft.Extensions.DependencyInjection.ServiceInitialization.Extensions;

namespace ModularPipelines.Host;

public class PipelineHostBuilder : IPipelineHostBuilder
{
    private readonly IHostBuilder _internalHost;

    internal PipelineHostBuilder()
    {
        _internalHost = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder();
        _internalHost.ConfigureServices(services =>
        {
            services
                .Configure<PipelineOptions>(_ => {})
                .AddLogging()
                .AddHttpClient()
                .AddInitializers()
                .AddSingleton<IPipelineInitializer, PipelineInitializer>()
                .AddSingleton<IPipelineSetupExecutor, PipelineSetupExecutor>()
                .AddSingleton<IModuleIgnoreHandler, ModuleIgnoreHandler>()
                .AddSingleton<IPipelineConsolePrinter, PipelineConsolePrinter>()
                .AddSingleton<IPipelineExecutor, PipelineExecutor>()
                .AddSingleton<IModuleContext, ModuleContext>()
                .AddSingleton<IDependencyCollisionDetector, DependencyCollisionDetector>()
                .AddSingleton<IEnvironmentContext, EnvironmentContext>()
                .AddSingleton<IFileSystemContext, FileSystemContext>()
                .AddSingleton<IRequirementChecker, RequirementChecker>();
        });
    }

    public static IPipelineHostBuilder Create() => new PipelineHostBuilder();
    
    public IPipelineHostBuilder ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate)
    {
        _internalHost.ConfigureAppConfiguration(configureDelegate);
        return this;
    }

    public IPipelineHostBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
    {
        _internalHost.ConfigureServices(configureDelegate);
        return this;
    }
    
    public IPipelineHostBuilder ConfigurePipelineOptions(Action<HostBuilderContext, PipelineOptions> configureDelegate)
    {
        _internalHost.ConfigureServices((context, collection) =>
        {
            
            collection.Configure<PipelineOptions>(options => configureDelegate(context, options));
        });
        
        return this;
    }

    public async Task<IModule[]> ExecutePipelineAsync()
    {
        var host = _internalHost.Build();
        await host.Services.GetRequiredService<IPipelineInitializer>().InitializeAsync();
        return await host.Services.GetRequiredService<IPipelineExecutor>().ExecuteAsync();
    }
}