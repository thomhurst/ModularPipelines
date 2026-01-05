using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModularPipelines.DependencyInjection;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Interfaces;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.Requirements;
using ModularPipelines.Validation;
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
    /// Used to configure the pipeline options for the entire pipeline.
    /// </summary>
    /// <param name="configureDelegate">A delegate to amend the options.</param>
    /// <returns>The same pipeline host builder.</returns>
    /// <remarks>
    /// <para>
    /// <strong>Configuration Precedence:</strong>
    /// Settings configured here represent the "Global Configuration" level in the precedence hierarchy.
    /// These settings apply as defaults across the pipeline but can be overridden at more specific levels.
    /// </para>
    /// <para>
    /// <strong>Precedence order (highest to lowest):</strong>
    /// </para>
    /// <list type="number">
    /// <item>Per-Call Configuration - Options passed to individual method calls (e.g., <see cref="CommandExecutionOptions.LogSettings"/>)</item>
    /// <item>Module Configuration - Behavior interfaces on modules (e.g., <see cref="Modules.Behaviors.IRetryable{T}"/>, <see cref="Modules.Behaviors.ITimeoutable"/>)</item>
    /// <item>Global Configuration - Settings in <see cref="PipelineOptions"/> configured here</item>
    /// <item>System Defaults - Built-in framework defaults (e.g., 30-minute module timeout)</item>
    /// </list>
    /// <para>
    /// Example: If <see cref="PipelineOptions.DefaultRetryCount"/> is set to 3, all modules will retry
    /// up to 3 times on failure. However, a module implementing <see cref="Modules.Behaviors.IRetryable{T}"/>
    /// will use its custom retry policy instead.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// PipelineHostBuilder.Create()
    ///     .ConfigurePipelineOptions((context, options) =>
    ///     {
    ///         options.DefaultRetryCount = 3;
    ///         options.DefaultLoggingOptions = CommandLoggingOptions.Diagnostic;
    ///     })
    ///     .AddModule&lt;MyModule&gt;()
    ///     .ExecutePipelineAsync();
    /// </code>
    /// </example>
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
        var host = await BuildHostAsync().ConfigureAwait(false);
        await using (host.ConfigureAwait(false))
        {
            return await host.ExecutePipelineAsync().ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Builds the pipeline host and validates configuration.
    /// This separates the build phase from execution, allowing early detection of configuration errors.
    /// </summary>
    /// <returns>A validated pipeline host ready for execution.</returns>
    /// <exception cref="PipelineValidationException">Thrown when validation fails with configuration errors.</exception>
    /// <example>
    /// <code>
    /// var host = await builder.BuildAsync(); // Error here if misconfigured
    /// var result = await host.RunAsync();    // Only executes if valid
    /// </code>
    /// </example>
    public async Task<IPipelineHost> BuildAsync()
    {
        IPipelineHost? host = null;
        ValidationResult validationResult;

        try
        {
            host = await BuildHostAsync().ConfigureAwait(false);
            validationResult = ValidateHost(host.Services);
        }
        catch (PipelineException ex) when (ex.Message.Contains("No modules"))
        {
            // Convert "no modules" exception to validation error
            validationResult = ValidationResult.WithError(new ValidationError(
                ValidationErrorCategory.ModuleConfiguration,
                "No modules are registered. A pipeline must have at least one module."));
        }
        catch (ModuleNotRegisteredException ex)
        {
            // Convert missing dependency exception to validation error
            validationResult = ValidationResult.WithError(new ValidationError(
                ValidationErrorCategory.Dependency,
                ex.Message));
        }

        if (validationResult.HasErrors)
        {
            if (host != null)
            {
                await host.DisposeAsync().ConfigureAwait(false);
            }

            throw new PipelineValidationException(validationResult);
        }

        return host!;
    }

    /// <summary>
    /// Validates the pipeline configuration without executing it.
    /// Use this for dry-run validation to catch configuration errors early.
    /// </summary>
    /// <returns>A validation result containing any errors found.</returns>
    /// <example>
    /// <code>
    /// var validationResults = await builder.ValidateAsync();
    /// if (validationResults.HasErrors)
    /// {
    ///     foreach (var error in validationResults.Errors)
    ///         Console.WriteLine($"  - {error.Message}");
    /// }
    /// </code>
    /// </example>
    public async Task<ValidationResult> ValidateAsync()
    {
        IPipelineHost? host = null;

        try
        {
            host = await BuildHostAsync().ConfigureAwait(false);
            return ValidateHost(host.Services);
        }
        catch (PipelineException ex) when (ex.Message.Contains("No modules"))
        {
            // Convert "no modules" exception to validation error
            return ValidationResult.WithError(new ValidationError(
                ValidationErrorCategory.ModuleConfiguration,
                "No modules are registered. A pipeline must have at least one module."));
        }
        catch (ModuleNotRegisteredException ex)
        {
            // Convert missing dependency exception to validation error
            return ValidationResult.WithError(new ValidationError(
                ValidationErrorCategory.Dependency,
                ex.Message));
        }
        finally
        {
            if (host != null)
            {
                await host.DisposeAsync().ConfigureAwait(false);
            }
        }
    }

    private static ValidationResult ValidateHost(IServiceProvider services)
    {
        var validationService = services.GetService<IPipelineValidationService>();
        return validationService?.Validate(services) ?? ValidationResult.Success();
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

        return await PipelineHost.Create(_internalHost).ConfigureAwait(false);
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
