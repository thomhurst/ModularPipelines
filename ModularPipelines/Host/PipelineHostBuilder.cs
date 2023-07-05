using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Helpers;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using TomLonghurst.Microsoft.Extensions.DependencyInjection.ServiceInitialization.Extensions;

namespace ModularPipelines.Host;

public class PipelineHostBuilder : IPipelineHostBuilder
{
    private readonly IHostBuilder _internalHost;
    private readonly PipelineEngineOverrides _overrides;

    internal PipelineHostBuilder()
    {
        _internalHost = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder();
        _overrides = new PipelineEngineOverrides( _internalHost );
        _internalHost.ConfigureServices( services =>
        {
            // Bundles
            services
                .Configure<PipelineOptions>( _ => { } )
                .AddLogging()
                .AddHttpClient()
                .AddInitializers();

            // Transient
            services.AddTransient<IModuleContext, ModuleContext>()
                .AddTransient<IModuleLoggerProvider, ModuleLoggerProvider>()
                .AddTransient<IHttp, Http>()
                .AddTransient<ICommand, Command>()
                .AddTransient<ICertificates, Certificates>()
                .AddTransient<IDownloader, Downloader>()
                .AddTransient<IInstaller, Installer>()
                .AddTransient<IHasher, Hasher>()
                .AddTransient<IBase64, Base64>()
                .AddTransient<IHex, Hex>()
                .AddTransient<IZip, Zip>()
                .AddTransient<IJson, Json>()
                .AddTransient<IXml, Xml>();

            // Singletons
            services
                .AddSingleton<EngineCancellationToken>()
                .AddSingleton<IPipelineInitializer, PipelineInitializer>()
                .AddSingleton<IPipelineSetupExecutor, PipelineSetupExecutor>()
                .AddSingleton<IModuleInitializer, ModuleInitializer>()
                .AddSingleton<IModuleIgnoreHandler, ModuleIgnoreHandler>()
                .AddSingleton<IPipelineConsolePrinter, PipelineConsoleProgressPrinter>()
                .AddSingleton<IPipelineExecutor, PipelineExecutor>()
                .AddSingleton<IModuleExecutor, ModuleExecutor>()
                .AddSingleton( typeof( ModuleLogger<> ) )
                .AddSingleton<IModuleLoggerContainer, ModuleLoggerContainer>()
                .AddSingleton<IDependencyChainProvider, DependencyChainProvider>()
                .AddSingleton<IDependencyDetector, DependencyDetector>()
                .AddSingleton<IDependencyCollisionDetector, DependencyCollisionDetector>()
                .AddSingleton<IDependencyPrinter, DependencyPrinter>()
                .AddSingleton<IEnvironmentContext, EnvironmentContext>()
                .AddSingleton<IEnvironmentVariables, EnvironmentVariables>()
                .AddSingleton<IFileSystemContext, FileSystemContext>()
                .AddSingleton<IRequirementChecker, RequirementChecker>()
                .AddSingleton<IModuleRetriever, ModuleRetriever>()
                .AddSingleton<IModuleResultPrinter, ModuleResultPrinter>()
                .AddSingleton<IModuleResultRepository, NoOpModuleResultRepository>()
                .AddSingleton<IModuleEstimatedTimeProvider, FileSystemModuleEstimatedTimeProvider>();
        } );
    }

    public static IPipelineHostBuilder Create() => new PipelineHostBuilder();

    public IPipelineHostBuilder ConfigureAppConfiguration( Action<HostBuilderContext, IConfigurationBuilder> configureDelegate )
    {
        _internalHost.ConfigureAppConfiguration( configureDelegate );
        return this;
    }

    public IPipelineHostBuilder ConfigureServices( Action<HostBuilderContext, IServiceCollection> configureDelegate )
    {
        _internalHost.ConfigureServices( configureDelegate );
        return this;
    }

    public IPipelineHostBuilder ConfigurePipelineOptions( Action<HostBuilderContext, PipelineOptions> configureDelegate )
    {
        _internalHost.ConfigureServices( ( context, collection ) =>
        {
            collection.Configure<PipelineOptions>( options => configureDelegate( context, options ) );
        } );

        return this;
    }

    public IPipelineHostBuilder ConfigureOverrides( Action<PipelineEngineOverrides> configureDelegate )
    {
        configureDelegate( _overrides );
        return this;
    }

    public async Task<IReadOnlyList<ModuleBase>> ExecutePipelineAsync()
    {
        LoadModularPipelineAssembliesIfNotLoadedYet();

        _internalHost.ConfigureServices( collection =>
        {
            foreach (var contextRegistrationDelegate in ModularPipelinesContextRegistry.ContextRegistrationDelegates)
            {
                contextRegistrationDelegate( collection );
            }
        } );

        var host = _internalHost.Build();

        await host.Services.GetRequiredService<IPipelineInitializer>().InitializeAsync();

        try
        {
            return await host.Services.GetRequiredService<IPipelineExecutor>().ExecuteAsync();
        }
        finally
        {
            await ((ServiceProvider) host.Services).DisposeAsync();
        }
    }

    private void LoadModularPipelineAssembliesIfNotLoadedYet()
    {
        var currentAssemblies = AppDomain.CurrentDomain.GetAssemblies();

        var unloadedModularPipelineAssemblies = GetDlls()
            .Select( Path.GetFileNameWithoutExtension )
            .Except( currentAssemblies.Select( x => x.GetName().Name ) )
            .OfType<string>()
            .ToList();

        foreach (var modularPipelineAssembly in unloadedModularPipelineAssemblies)
        {
            Assembly.Load( new AssemblyName( modularPipelineAssembly ) );
        }

        foreach (var assembly in AppDomain.CurrentDomain
                     .GetAssemblies()
                     .Where( a => a.GetName().Name?.Contains( "ModularPipeline", StringComparison.InvariantCultureIgnoreCase ) == true ))
        {
            RuntimeHelpers.RunModuleConstructor( assembly.ManifestModule.ModuleHandle );
        }
    }

    private static IEnumerable<string> GetDlls()
    {
        var baseDirectoryDlls = Directory.EnumerateFiles( AppDomain.CurrentDomain.BaseDirectory, "*ModularPipeline*.dll", SearchOption.TopDirectoryOnly );

        if (string.IsNullOrEmpty( AppDomain.CurrentDomain.DynamicDirectory ))
        {
            return baseDirectoryDlls;
        }

        return baseDirectoryDlls
            .Concat( Directory.EnumerateFiles( AppDomain.CurrentDomain.DynamicDirectory, "*ModularPipeline*.dll", SearchOption.TopDirectoryOnly ) )
            .Distinct();

    }
}
