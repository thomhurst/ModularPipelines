using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModularPipelines.DependencyInjection;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.Requirements;
using TomLonghurst.Microsoft.Extensions.DependencyInjection.ServiceInitialization.Extensions;

namespace ModularPipelines.Host;

public class PipelineHostBuilder
{
    private readonly IHostBuilder _internalHost;
    private readonly PipelineEnginePlugins _plugins;
    
    public PipelineHostBuilder()
    {
        _internalHost = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder();
        _plugins = new PipelineEnginePlugins(_internalHost);
        _internalHost.ConfigureServices(DependencyInjectionSetup.Initialize);
    }

    public static PipelineHostBuilder Create() => new();

    public PipelineHostBuilder ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate)
    {
        _internalHost.ConfigureAppConfiguration(configureDelegate);
        return this;
    }

    public PipelineHostBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
    {
        _internalHost.ConfigureServices(configureDelegate);
        return this;
    }

    public PipelineHostBuilder ConfigurePipelineOptions(Action<HostBuilderContext, PipelineOptions> configureDelegate)
    {
        _internalHost.ConfigureServices((context, collection) =>
        {
            collection.Configure<PipelineOptions>(options => configureDelegate(context, options));
        });

        return this;
    }
    
    public PipelineHostBuilder AddModule<TModule>() where TModule : ModuleBase
    {
        _internalHost.ConfigureServices((_, collection) =>
        {
            collection.AddModule<TModule>();
        });

        return this;
    }

    public PipelineHostBuilder AddRequirement<TRequirement>() where TRequirement : class, IPipelineRequirement
    {
        _internalHost.ConfigureServices((_, collection) =>
        {
            collection.AddRequirement<TRequirement>();
        });

        return this;
    }

    public PipelineHostBuilder RegisterEstimatedTimeProvider<TEstimatedTimeProvider>() where TEstimatedTimeProvider : class, IModuleEstimatedTimeProvider
    {
        _internalHost.ConfigureServices((_, collection) =>
        {
            collection.AddScoped<IModuleEstimatedTimeProvider, TEstimatedTimeProvider>();
        });

        return this;
    }

    public PipelineHostBuilder ConfigurePlugins(Action<PipelineEnginePlugins> configureDelegate)
    {
        configureDelegate(_plugins);
        return this;
    }

    public async Task<PipelineSummary> ExecutePipelineAsync()
    {
        var host = await BuildHostAsync();

        return await host.ExecutePipelineAsync();
    }

    internal async Task<IPipelineHost> BuildHostAsync()
    {
        LoadModularPipelineAssembliesIfNotLoadedYet();

        _internalHost.ConfigureServices(collection =>
        {
            foreach (var contextRegistrationDelegate in ModularPipelinesContextRegistry.ContextRegistrationDelegates)
            {
                contextRegistrationDelegate(collection);
            }
        });

        var host = new PipelineHost(_internalHost.Build());

        await host.RootServices.InitializeAsync();
        await host.RootServices.GetRequiredService<IServiceProviderInitializer>().InitializeAsync();
        
        return host;
    }

    private void LoadModularPipelineAssembliesIfNotLoadedYet()
    {
        var currentAssemblies = AppDomain.CurrentDomain.GetAssemblies();

        var unloadedModularPipelineAssemblies = GetDlls()
            .Select(Path.GetFileNameWithoutExtension)
            .Except(currentAssemblies.Select(x => x.GetName().Name))
            .OfType<string>()
            .ToList();

        foreach (var modularPipelineAssembly in unloadedModularPipelineAssemblies)
        {
            Assembly.Load(new AssemblyName(modularPipelineAssembly));
        }

        foreach (var assembly in AppDomain.CurrentDomain
                     .GetAssemblies()
                     .Where(a => a.GetName().Name?.Contains("ModularPipeline", StringComparison.InvariantCultureIgnoreCase) == true))
        {
            RuntimeHelpers.RunModuleConstructor(assembly.ManifestModule.ModuleHandle);
        }
    }

    private static IEnumerable<string> GetDlls()
    {
        var baseDirectoryDlls = Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory, "*ModularPipeline*.dll", SearchOption.TopDirectoryOnly);

        if (string.IsNullOrEmpty(AppDomain.CurrentDomain.DynamicDirectory))
        {
            return baseDirectoryDlls;
        }

        return baseDirectoryDlls
            .Concat(Directory.EnumerateFiles(AppDomain.CurrentDomain.DynamicDirectory, "*ModularPipeline*.dll", SearchOption.TopDirectoryOnly))
            .Distinct();
    }
}