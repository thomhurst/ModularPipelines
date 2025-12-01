using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModularPipelines.DependencyInjection;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Interfaces;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.Requirements;
using Vertical.SpectreLogger.Options;

namespace ModularPipelines.Host;

/// <summary>
/// A builder to configure and then execute ModularPipelines.
/// </summary>
public class PipelineHostBuilder
{
    private readonly IHostBuilder _internalHost;

    /// <summary>
    /// Initialises a new instance of the <see cref="PipelineHostBuilder"/> class.
    /// </summary>
    public PipelineHostBuilder()
    {
        _internalHost = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder();
        _internalHost.ConfigureServices(DependencyInjectionSetup.Initialize);
    }

    /// <summary>
    /// Creates a <see cref="PipelineHostBuilder"/>.
    /// </summary>
    /// <returns><see cref="PipelineHostBuilder"/>.</returns>
    public static PipelineHostBuilder Create() => new();

    /// <summary>
    /// Used to configure the configuration.
    /// </summary>
    /// <param name="configureDelegate">A delegate to amend the configuration.</param>
    /// <returns>The same pipeline host builder.</returns>
    public PipelineHostBuilder ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate)
    {
        _internalHost.ConfigureAppConfiguration(configureDelegate);
        return this;
    }

    /// <summary>
    /// Used to configure the services and dependency injection within the pipeline.
    /// </summary>
    /// <param name="configureDelegate">A delegate to amend the services.</param>
    /// <returns>The same pipeline host builder.</returns>
    public PipelineHostBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
    {
        _internalHost.ConfigureServices(configureDelegate);
        return this;
    }

    /// <summary>
    /// Used to configure the pipeline options.
    /// </summary>
    /// <param name="configureDelegate">A delegate to amend the options.</param>
    /// <returns>The same pipeline host builder.</returns>
    public PipelineHostBuilder ConfigurePipelineOptions(Action<HostBuilderContext, PipelineOptions> configureDelegate)
    {
        _internalHost.ConfigureServices((context, collection) =>
        {
            collection.Configure<PipelineOptions>(options => configureDelegate(context, options));
        });

        return this;
    }

    /// <summary>
    /// Registers a module to be added to the pipeline.
    /// </summary>
    /// <typeparam name="TModule">The type of module.</typeparam>
    /// <returns>The same pipeline host builder.</returns>
    public PipelineHostBuilder AddModule<TModule>()
        where TModule : class, IModule
    {
        _internalHost.ConfigureServices((_, collection) =>
        {
            collection.AddModule<TModule>();
        });

        return this;
    }

    /// <summary>
    /// Registers hooks to be executed before all modules have started, and after all modules have finished.
    /// </summary>
    /// <typeparam name="TGlobalHooks">The type of hooks.</typeparam>
    /// <returns>The same pipeline host builder.</returns>
    public PipelineHostBuilder AddGlobalHooks<TGlobalHooks>()
        where TGlobalHooks : class, IPipelineGlobalHooks
    {
        _internalHost.ConfigureServices((_, collection) =>
        {
            collection.AddSingleton<IPipelineGlobalHooks, TGlobalHooks>();
        });

        return this;
    }

    /// <summary>
    /// Registers hooks to be executed before and after each module is executed.
    /// </summary>
    /// <typeparam name="TModuleHooks">The type of hooks.</typeparam>
    /// <returns>The same pipeline host builder.</returns>
    public PipelineHostBuilder AddModuleHooks<TModuleHooks>()
        where TModuleHooks : class, IPipelineModuleHooks
    {
        _internalHost.ConfigureServices((_, collection) =>
        {
            collection.AddSingleton<IPipelineModuleHooks, TModuleHooks>();
        });

        return this;
    }

    /// <summary>
    /// Registers classes that will write a pipeline output file for a build system to interpret.
    /// </summary>
    /// <typeparam name="TBuildSystemPipelineFileWriter">The type of Build System Pipeline File Writer.</typeparam>
    /// <returns>The same pipeline host builder.</returns>
    public PipelineHostBuilder AddPipelineFileWriter<TBuildSystemPipelineFileWriter>()
        where TBuildSystemPipelineFileWriter : class, IBuildSystemPipelineFileWriter
    {
        _internalHost.ConfigureServices((_, collection) =>
        {
            collection.AddScoped<IBuildSystemPipelineFileWriter, TBuildSystemPipelineFileWriter>();
        });

        return this;
    }

    /// <summary>
    /// Adds a check when initialising the pipeline, failing if the requirement isn't met.
    /// </summary>
    /// <typeparam name="TRequirement">The type of requirement.</typeparam>
    /// <returns>The same pipeline host builder.</returns>
    public PipelineHostBuilder AddRequirement<TRequirement>()
        where TRequirement : class, IPipelineRequirement
    {
        _internalHost.ConfigureServices((_, collection) =>
        {
            collection.AddRequirement<TRequirement>();
        });

        return this;
    }

    /// <summary>
    /// Modules with the specified categories will be run, and any other categories ignored.
    /// </summary>
    /// <param name="categories">An array of any categories to be run.</param>
    /// <returns>The same pipeline host builder.</returns>
    public PipelineHostBuilder RunCategories(params string[] categories)
    {
        return ConfigurePipelineOptions((_, options) =>
        {
            options.RunOnlyCategories ??= new List<string>();

            foreach (var category in categories)
            {
                options.RunOnlyCategories.Add(category);
            }
        });
    }

    /// <summary>
    /// Modules with the specified categories will not be run.
    /// </summary>
    /// <param name="categories">An array of any categories to not be run.</param>
    /// <returns>The same pipeline host builder.</returns>
    public PipelineHostBuilder IgnoreCategories(params string[] categories)
    {
        return ConfigurePipelineOptions((_, options) =>
        {
            options.IgnoreCategories ??= new List<string>();

            foreach (var category in categories)
            {
                options.IgnoreCategories.Add(category);
            }
        });
    }

    /// <summary>
    /// Runs the pipelines.
    /// </summary>
    /// <returns>A summary of the pipeline results.</returns>
    public async Task<PipelineSummary> ExecutePipelineAsync()
    {
        await using var host = await BuildHostAsync();

        return await host.ExecutePipelineAsync();
    }

    /// <summary>
    /// Registers a repository used to store and retrieve the time taken to run a module.
    /// </summary>
    /// <typeparam name="T">The type of repository.</typeparam>
    /// <returns>The same pipeline host builder.</returns>
    public PipelineHostBuilder AddModuleEstimatedTimeProvider<T>()
        where T : class, IModuleEstimatedTimeProvider
    {
        return OverrideGeneric<IModuleEstimatedTimeProvider, T>();
    }

    /// <summary>
    /// Registers a repository used to store and retrieve module results.
    /// </summary>
    /// <typeparam name="T">The type of repository.</typeparam>
    /// <returns>The same pipeline host builder.</returns>
    public PipelineHostBuilder AddResultsRepository<T>()
        where T : class, IModuleResultRepository
    {
        return OverrideGeneric<IModuleResultRepository, T>();
    }

    /// <summary>
    /// Configures the log level for the pipeline.
    /// </summary>
    /// <param name="logLevel">The log level to set.</param>
    /// <returns>The same pipeline host builder.</returns>
    public PipelineHostBuilder SetLogLevel(LogLevel logLevel)
    {
        _internalHost.ConfigureServices(collection =>
        {
            collection.Configure<SpectreLoggerOptions>(options => options.MinimumLogLevel = logLevel);
            collection.Configure<LoggerFilterOptions>(options => options.MinLevel = logLevel);
        });

        return this;
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

        return await PipelineHost.Create(_internalHost);
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

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
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

    private PipelineHostBuilder OverrideGeneric<TBase, T>()
        where T : class, TBase
        where TBase : class
    {
        _internalHost.ConfigureServices(s =>
        {
            s.RemoveAll<TBase>()
                .AddSingleton<TBase, T>();
        });

        return this;
    }
}