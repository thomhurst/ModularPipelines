using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModularPipelines.DependencyInjection;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Options;
using ModularPipelines.Plugins;
using ModularPipelines.Validation;
using Vertical.SpectreLogger.Options;

namespace ModularPipelines;

/// <summary>
/// A builder for configuring and creating a pipeline.
/// </summary>
public sealed class PipelineBuilder
{
    private readonly IHostBuilder _hostBuilder;
    private readonly ServiceCollection _services;
    private readonly ConfigurationManager _configuration;
    private readonly PipelineOptions _options;

    internal PipelineBuilder(string[]? args)
    {
        _services = new ServiceCollection();
        _configuration = new ConfigurationManager();
        _options = new PipelineOptions();

        _hostBuilder = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args);

        // Add default configuration sources
        _configuration.AddEnvironmentVariables();
        if (args != null)
        {
            _configuration.AddCommandLine(args);
        }
    }

    internal PipelineBuilder(PipelineBuilderOptions options) : this(options.Args)
    {
        if (!string.IsNullOrEmpty(options.EnvironmentName))
        {
            _hostBuilder.UseEnvironment(options.EnvironmentName);
        }

        if (!string.IsNullOrEmpty(options.ContentRootPath))
        {
            _hostBuilder.UseContentRoot(options.ContentRootPath);
        }
    }

    /// <summary>
    /// Gets the service collection for registering services and modules.
    /// </summary>
    public IServiceCollection Services => _services;

    /// <summary>
    /// Gets the configuration manager for adding and reading configuration.
    /// </summary>
    public ConfigurationManager Configuration => _configuration;

    /// <summary>
    /// Gets the pipeline options for configuring execution behavior.
    /// </summary>
    public PipelineOptions Options => _options;

    /// <summary>
    /// Gets the host environment information.
    /// </summary>
    public IHostEnvironment Environment
    {
        get
        {
            // Build a temporary host to get the environment
            // This is a lightweight operation as we're not running anything
            using var tempHost = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder().Build();
            return tempHost.Services.GetRequiredService<IHostEnvironment>();
        }
    }

    /// <summary>
    /// Sets the minimum log level for the pipeline.
    /// </summary>
    /// <param name="logLevel">The minimum log level.</param>
    /// <returns>The same builder instance for chaining.</returns>
    public PipelineBuilder SetLogLevel(LogLevel logLevel)
    {
        _services.Configure<SpectreLoggerOptions>(options => options.MinimumLogLevel = logLevel);
        _services.Configure<LoggerFilterOptions>(options => options.MinLevel = logLevel);
        return this;
    }

    /// <summary>
    /// Configures modules with the specified categories to be run exclusively.
    /// </summary>
    /// <param name="categories">The categories to run.</param>
    /// <returns>The same builder instance for chaining.</returns>
    public PipelineBuilder RunCategories(params string[] categories)
    {
        _options.RunOnlyCategories ??= new List<string>();
        foreach (var category in categories)
        {
            _options.RunOnlyCategories.Add(category);
        }

        return this;
    }

    /// <summary>
    /// Configures modules with the specified categories to be ignored.
    /// </summary>
    /// <param name="categories">The categories to ignore.</param>
    /// <returns>The same builder instance for chaining.</returns>
    public PipelineBuilder IgnoreCategories(params string[] categories)
    {
        _options.IgnoreCategories ??= new List<string>();
        foreach (var category in categories)
        {
            _options.IgnoreCategories.Add(category);
        }

        return this;
    }

    /// <summary>
    /// Builds the pipeline and validates configuration.
    /// </summary>
    /// <returns>A validated pipeline ready for execution.</returns>
    /// <exception cref="PipelineValidationException">Thrown when validation fails.</exception>
    public async Task<IPipeline> BuildAsync()
    {
        IPipeline? pipeline = null;
        ValidationResult validationResult;

        try
        {
            pipeline = await BuildPipelineAsync().ConfigureAwait(false);
            validationResult = ValidatePipeline(pipeline.Services);
        }
        catch (PipelineException ex) when (ex.Message.Contains("No modules"))
        {
            validationResult = ValidationResult.WithError(new ValidationError(
                ValidationErrorCategory.ModuleConfiguration,
                "No modules are registered. A pipeline must have at least one module."));
        }
        catch (ModuleNotRegisteredException ex)
        {
            validationResult = ValidationResult.WithError(new ValidationError(
                ValidationErrorCategory.Dependency,
                ex.Message));
        }
        catch (ModuleReferencingSelfException ex)
        {
            validationResult = ValidationResult.WithError(new ValidationError(
                ValidationErrorCategory.Dependency,
                ex.Message));
        }
        catch (DependencyCollisionException ex)
        {
            validationResult = ValidationResult.WithError(new ValidationError(
                ValidationErrorCategory.Dependency,
                ex.Message));
        }

        if (validationResult.HasErrors)
        {
            if (pipeline != null)
            {
                await pipeline.DisposeAsync().ConfigureAwait(false);
            }

            throw new PipelineValidationException(validationResult);
        }

        return pipeline!;
    }

    /// <summary>
    /// Builds the pipeline without validation.
    /// </summary>
    /// <returns>A pipeline ready for execution.</returns>
    public IPipeline Build()
    {
        return BuildPipelineAsync().GetAwaiter().GetResult();
    }

    /// <summary>
    /// Validates the pipeline configuration without executing it.
    /// </summary>
    /// <returns>A validation result containing any errors found.</returns>
    public async Task<ValidationResult> ValidateAsync()
    {
        IPipeline? pipeline = null;

        try
        {
            pipeline = await BuildPipelineAsync().ConfigureAwait(false);
            return ValidatePipeline(pipeline.Services);
        }
        catch (PipelineException ex) when (ex.Message.Contains("No modules"))
        {
            return ValidationResult.WithError(new ValidationError(
                ValidationErrorCategory.ModuleConfiguration,
                "No modules are registered. A pipeline must have at least one module."));
        }
        catch (ModuleNotRegisteredException ex)
        {
            return ValidationResult.WithError(new ValidationError(
                ValidationErrorCategory.Dependency,
                ex.Message));
        }
        catch (ModuleReferencingSelfException ex)
        {
            return ValidationResult.WithError(new ValidationError(
                ValidationErrorCategory.Dependency,
                ex.Message));
        }
        catch (DependencyCollisionException ex)
        {
            return ValidationResult.WithError(new ValidationError(
                ValidationErrorCategory.Dependency,
                ex.Message));
        }
        finally
        {
            if (pipeline != null)
            {
                await pipeline.DisposeAsync().ConfigureAwait(false);
            }
        }
    }

    private static ValidationResult ValidatePipeline(IServiceProvider services)
    {
        var validationService = services.GetService<IPipelineValidationService>();
        return validationService?.Validate(services) ?? ValidationResult.Success();
    }

    private async Task<IPipeline> BuildPipelineAsync()
    {
        LoadModularPipelineAssembliesIfNotLoadedYet();

        // Apply plugin configuration to the builder (modules, hooks, options)
        PluginIntegration.ApplyPluginConfiguration(this);

        // Configure the host with our collected configuration
        _hostBuilder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddConfiguration(_configuration);
        });

        // Configure services: core first, then user services, then plugins (so plugins can inspect user config)
        _hostBuilder.ConfigureServices((_, services) =>
        {
            DependencyInjectionSetup.Initialize(services);

            // Add user-registered services before plugins so plugins can inspect user configuration
            // (e.g., DistributedPipelinePlugin needs to find IOptions<DistributedOptions>)
            foreach (var descriptor in _services)
            {
                services.Add(descriptor);
            }

            // Apply plugin services after user services
            PluginIntegration.ApplyPluginServices(services);

            // Configure pipeline options
            services.Configure<PipelineOptions>(opts =>
            {
                opts.ExecutionMode = _options.ExecutionMode;
                opts.RunOnlyCategories = _options.RunOnlyCategories;
                opts.IgnoreCategories = _options.IgnoreCategories;
                opts.ShowProgressInConsole = _options.ShowProgressInConsole;
                opts.PrintResults = _options.PrintResults;
                opts.PrintLogo = _options.PrintLogo;
                opts.PrintDependencyChains = _options.PrintDependencyChains;
                opts.DefaultRetryCount = _options.DefaultRetryCount;
                opts.DefaultLoggingOptions = _options.DefaultLoggingOptions;
                opts.DefaultHttpLoggingOptions = _options.DefaultHttpLoggingOptions;
                opts.DefaultHttpTimeout = _options.DefaultHttpTimeout;
                opts.DefaultHttpResilienceOptions = _options.DefaultHttpResilienceOptions;
                opts.Concurrency = _options.Concurrency;
                opts.ConsoleWidth = _options.ConsoleWidth;
                opts.DefaultExecutionOptions = _options.DefaultExecutionOptions;
                opts.ThrowOnPipelineFailure = _options.ThrowOnPipelineFailure;
            });

            // Auto-register any missing required dependencies
            ModuleAutoRegistrar.AutoRegisterMissingDependencies(services);

            // Register context delegates from loaded ModularPipeline assemblies
            foreach (var contextRegistrationDelegate in ModularPipelinesContextRegistry.ContextRegistrationDelegates)
            {
                contextRegistrationDelegate(services);
            }
        });

        return await PipelineImpl.CreateAsync(_hostBuilder).ConfigureAwait(false);
    }

    private void LoadModularPipelineAssembliesIfNotLoadedYet()
    {
        var coreVersion = typeof(PipelineBuilder).Assembly.GetName().Version;
        var currentAssemblies = AppDomain.CurrentDomain.GetAssemblies();

        var unloadedModularPipelineAssemblies = GetDlls()
            .Select(Path.GetFileNameWithoutExtension)
            .Except(currentAssemblies.Select(x => x.GetName().Name))
            .OfType<string>()
            .ToList();

        foreach (var modularPipelineAssembly in unloadedModularPipelineAssemblies)
        {
            var assembly = Assembly.Load(new AssemblyName(modularPipelineAssembly));
            PluginVersionValidator.Validate(assembly, coreVersion);
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
}
