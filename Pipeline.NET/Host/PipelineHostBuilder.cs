using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pipeline.NET.Context;
using Pipeline.NET.Engine;
using Pipeline.NET.Helpers;
using Pipeline.NET.Interfaces;
using Pipeline.NET.Modules;
using Pipeline.NET.Options;
using TomLonghurst.Microsoft.Extensions.DependencyInjection.ServiceInitialization.Extensions;

namespace Pipeline.NET.Host;

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

    public PipelineHostBuilder AddModule<TModule>() where TModule : class, IModule
    {
        _internalHost.ConfigureServices(services => services.AddSingleton<IModule, TModule>());
        return this;
    }
    
    public PipelineHostBuilder AddModule<TModule>(TModule tModule) where TModule : class, IModule
    {
        _internalHost.ConfigureServices(services => services.AddSingleton<IModule>(tModule));
        return this;
    }
    
    public PipelineHostBuilder AddModule<TModule>(Func<IServiceProvider, TModule> tModuleFactory) where TModule : class, IModule
    {
        _internalHost.ConfigureServices(services => services.AddSingleton<IModule>(tModuleFactory));
        return this;
    }
    
    public PipelineHostBuilder AddRequirement<TRequirement>() where TRequirement : class, IPipelineRequirement
    {
        _internalHost.ConfigureServices(services => services.AddSingleton<IPipelineRequirement, TRequirement>());
        return this;
    }
    
    public PipelineHostBuilder AddRequirement<TRequirement>(TRequirement tRequirement) where TRequirement : class, IPipelineRequirement
    {
        _internalHost.ConfigureServices(services => services.AddSingleton<IPipelineRequirement>(tRequirement));
        return this;
    }
    
    public PipelineHostBuilder AddRequirement<TRequirement>(Func<IServiceProvider, TRequirement> tRequirementFactory) where TRequirement : class, IPipelineRequirement
    {
        _internalHost.ConfigureServices(services => services.AddSingleton<IPipelineRequirement>(tRequirementFactory));
        return this;
    }
    
    public PipelineHostBuilder AddModulesFromAssemblyContainingType<T>()
    {
        return AddModulesFromAssembly(typeof(T).Assembly);
    }

    public PipelineHostBuilder AddModulesFromAssembly(Assembly assembly)
    {
        _internalHost.ConfigureServices(services =>
        {
            AddModulesFromAssembly(services, assembly);
        });

        return this;
    }

    public PipelineHostBuilder WithGlobalSetup<TGlobalSetup>() where TGlobalSetup : class, IPipelineGlobalSetup
    {
        _internalHost.ConfigureServices(services => services.AddSingleton<IPipelineGlobalSetup, TGlobalSetup>());

        return this;
    }

    public Task<IModule[]> ExecutePipelineAsync()
    {
        return _internalHost.Build().Services.GetRequiredService<IPipelineExecutor>().ExecuteAsync();
    }

    private static void AddModulesFromAssembly(IServiceCollection services, Assembly assembly)
    {
        var modules = assembly.GetTypes()
            .Where(type => type.IsAssignableTo(typeof(IModule)))
            .Where(type => type.IsClass)
            .Where(type => !type.IsAbstract);

        foreach (var module in modules)
        {
            services.AddSingleton(typeof(IModule), module);
        }
    }
}