using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.External;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Scrapers;
using ModularPipelines.OptionsGenerator.Scrapers.Base;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator;

internal static class OptionsGeneratorCommand
{
    public static async Task<int> RunAsync(string[] args)
    {
        var rootCommand = BuildRootCommand();
        var parseResult = rootCommand.Parse(args);
        return await parseResult.InvokeAsync();
    }

    private static RootCommand BuildRootCommand()
    {
        var options = CreateOptions();
        var rootCommand = new RootCommand("ModularPipelines CLI Options Generator");
        rootCommand.Options.Add(options.Tools);
        rootCommand.Options.Add(options.OutputDirectory);
        rootCommand.Options.Add(options.Input);
        rootCommand.Options.Add(options.UseCliFirst);
        rootCommand.Options.Add(options.EnhanceTypes);
        rootCommand.Options.Add(options.ApproveCommandCoverageShrinkage);
        rootCommand.Options.Add(options.ChangeManifest);
        rootCommand.Options.Add(options.ListTools);
        rootCommand.Options.Add(options.Json);
        rootCommand.SetAction((parseResult, cancellationToken) =>
            ExecuteAsync(ParseSettings(parseResult, options), cancellationToken));
        return rootCommand;
    }

    private static CommandOptions CreateOptions()
    {
        return new CommandOptions(
            Tools: new Option<string>("--tools", "-t")
            {
                Description = "Comma-separated list of tools to generate, or 'all'",
                DefaultValueFactory = _ => "all",
            },
            OutputDirectory: new Option<string>("--output-dir", "-o")
            {
                Description = "Root directory for generated output",
                DefaultValueFactory = _ => ".",
            },
            Input: new Option<string?>("--input", "-i")
            {
                Description = "Versioned JSON metadata for an external or private CLI integration",
            },
            UseCliFirst: new Option<bool>("--use-cli-first")
            {
                Description = "Use CLI --help parsing instead of HTML scraping (requires CLI tools to be installed). Recommended for Cobra CLIs (helm, docker, kubectl).",
                DefaultValueFactory = _ => true,
            },
            EnhanceTypes: new Option<bool>("--enhance-types")
            {
                Description = "Use CLI --help output to enhance type detection after scraping",
                DefaultValueFactory = _ => true,
            },
            ApproveCommandCoverageShrinkage: new Option<bool>("--approve-command-coverage-shrinkage")
            {
                Description = "Explicitly approve same-version command-set changes, removed commands, and command groups losing all children. Sentinel and minimum coverage checks still apply.",
                DefaultValueFactory = _ => false,
            },
            ChangeManifest: new Option<string?>("--change-manifest")
            {
                Description = "Write the repository-relative generated and deleted paths to this file",
            },
            ListTools: new Option<bool>("--list-tools")
            {
                Description = "List registered first-party CLI tools without generating files",
            },
            Json: new Option<bool>("--json")
            {
                Description = "Write machine-readable JSON when listing tools",
            });
    }

    private static GeneratorSettings ParseSettings(ParseResult parseResult, CommandOptions options)
    {
        return new GeneratorSettings(
            Tools: parseResult.GetValue(options.Tools) ?? "all",
            OutputDirectory: parseResult.GetValue(options.OutputDirectory) ?? ".",
            Input: parseResult.GetValue(options.Input),
            UseCliFirst: parseResult.GetValue(options.UseCliFirst),
            EnhanceTypes: parseResult.GetValue(options.EnhanceTypes),
            ApproveCommandCoverageShrinkage: parseResult.GetValue(options.ApproveCommandCoverageShrinkage),
            ChangeManifest: parseResult.GetValue(options.ChangeManifest),
            ListTools: parseResult.GetValue(options.ListTools),
            Json: parseResult.GetValue(options.Json));
    }

    private static async Task<int> ExecuteAsync(
        GeneratorSettings settings,
        CancellationToken cancellationToken)
    {
        if (settings.Json && !settings.ListTools)
        {
            Console.Error.WriteLine("--json requires --list-tools.");
            return 1;
        }

        if (settings.ListTools)
        {
            if (settings.Input is not null)
            {
                Console.Error.WriteLine("--list-tools cannot be combined with --input.");
                return 1;
            }

            var entries = CreateToolCatalog();
            Console.WriteLine(settings.Json ? ToolCatalog.ToJson(entries) : ToolCatalog.ToText(entries));
            return 0;
        }

        if (settings.Input is not null
            && !string.Equals(settings.Tools, "all", StringComparison.OrdinalIgnoreCase))
        {
            Console.Error.WriteLine("--input cannot be combined with --tools.");
            return 1;
        }

        ValidateExecutableOverride(settings);
        using var host = BuildHost(settings.EnhanceTypes);
        var logger = host.Services.GetRequiredService<ILoggerFactory>()
            .CreateLogger(nameof(OptionsGeneratorCommand));
        var orchestrator = CreateOrchestrator(host, settings.Input is not null);
        LogConfiguration(logger, settings);

        GenerationResult result;
        try
        {
            result = await GenerateAsync(orchestrator, settings, cancellationToken);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Generation failed");
            return 1;
        }

        await WriteChangeManifestAsync(result, settings.ChangeManifest, cancellationToken);
        return WriteResult(result);
    }

    private static void ValidateExecutableOverride(GeneratorSettings settings)
    {
        if (settings.Input is null)
        {
            ExecutableOverrideValidator.Validate(
                settings.Tools,
                Environment.GetEnvironmentVariable(ProcessCliCommandExecutor.ExecutableOverrideVariableName));
        }
    }

    private static IHost BuildHost(bool enhanceTypes, bool suppressLogs = false)
    {
        var builder = Host.CreateApplicationBuilder();
        builder.Logging.AddConsole();
        builder.Logging.SetMinimumLevel(suppressLogs ? LogLevel.None : LogLevel.Information);
        builder.Services.AddHttpClient();
        builder.Services.AddSingleton<ProcessCliCommandExecutor>();
        builder.Services.AddSingleton<ICliCommandExecutor>(serviceProvider =>
        {
            var inner = serviceProvider.GetRequiredService<ProcessCliCommandExecutor>();
            var logger = serviceProvider.GetRequiredService<ILogger<ResilientCliCommandExecutor>>();
            return new ResilientCliCommandExecutor(inner, logger);
        });
        builder.Services.AddSingleton<IHelpTextCache, HelpTextCache>();

        RegisterCliScrapers(builder.Services);
        RegisterDocumentationScrapers(builder.Services);
        RegisterGenerators(builder.Services);

        if (enhanceTypes)
        {
            builder.Services.AddSingleton(serviceProvider =>
            {
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                return OptionTypeEnhancer.CreateDefault(loggerFactory);
            });
        }

        builder.Services.AddSingleton<CodeGeneratorOrchestrator>();
        return builder.Build();
    }

    internal static IReadOnlyList<ToolCatalogEntry> CreateToolCatalog()
    {
        using var host = BuildHost(enhanceTypes: false, suppressLogs: true);
        return ToolCatalog.Create(host.Services.GetServices<ICliScraper>());
    }

    private static void RegisterCliScrapers(IServiceCollection services)
    {
        services.AddSingleton<ICliScraper, HelmCliScraper>();
        services.AddSingleton<ICliScraper, DockerCliScraper>();
        services.AddSingleton<ICliScraper, KubectlCliScraper>();
        services.AddSingleton<ICliScraper, KustomizeCliScraper>();
        services.AddSingleton<ICliScraper, GcloudCliScraper>();
        services.AddSingleton<ICliScraper, GitCliScraper>();
        services.AddSingleton<ICliScraper, TerraformCliScraper>();
        services.AddSingleton<ICliScraper, WinGetCliScraper>();
        services.AddSingleton<ICliScraper, ChocolateyCliScraper>();
        services.AddSingleton<ICliScraper, BrewCliScraper>();
        services.AddSingleton<ICliScraper, YarnCliScraper>();
        services.AddSingleton<ICliScraper, AwsCliScraper>();
        services.AddSingleton<ICliScraper, AzCliScraper>();
        services.AddSingleton<ICliScraper, DotNetCliScraper>();
        services.AddSingleton<ICliScraper, NpmCliScraper>();
        services.AddSingleton<ICliScraper, PnpmCliScraper>();
        services.AddSingleton<ICliScraper, GhCliScraper>();
        services.AddSingleton<ICliScraper, GoCliScraper>();
        services.AddSingleton<ICliScraper, NbgvCliScraper>();
        services.AddSingleton<ICliScraper, TrivyCliScraper>();
        services.AddSingleton<ICliScraper, PipCliScraper>();
        services.AddSingleton<ICliScraper, MavenCliScraper>();
        services.AddSingleton<ICliScraper, GradleCliScraper>();
        services.AddSingleton<ICliScraper, SonarScannerCliScraper>();
        services.AddSingleton<ICliScraper, SnykCliScraper>();
        services.AddSingleton<ICliScraper, HadolintCliScraper>();
        services.AddSingleton<ICliScraper, JqCliScraper>();
        services.AddSingleton<ICliScraper, YqCliScraper>();
        services.AddSingleton<ICliScraper, FlywayCliScraper>();
        services.AddSingleton<ICliScraper, LiquibaseCliScraper>();
        services.AddSingleton<ICliScraper, CargoCliScraper>();
        services.AddSingleton<ICliScraper, PulumiCliScraper>();
        services.AddSingleton<ICliScraper, PackerCliScraper>();
        services.AddSingleton<ICliScraper, VaultCliScraper>();
        services.AddSingleton<ICliScraper, AnsibleCliScraper>();
        services.AddSingleton<ICliScraper, PodmanCliScraper>();
        services.AddSingleton<ICliScraper, BuildahCliScraper>();
        services.AddSingleton<ICliScraper, SkopeoCliScraper>();
        services.AddSingleton<ICliScraper, EksctlCliScraper>();
        services.AddSingleton<ICliScraper, ArgoCdCliScraper>();
        services.AddSingleton<ICliScraper, FluxCliScraper>();
        services.AddSingleton<ICliScraper, ShellcheckCliScraper>();
        services.AddSingleton<ICliScraper, NewmanCliScraper>();
        services.AddSingleton<ICliScraper, KindCliScraper>();
        services.AddSingleton<ICliScraper, MinikubeCliScraper>();
        services.AddSingleton<ICliScraper, CosignCliScraper>();
        services.AddSingleton<ICliScraper, SyftCliScraper>();
        services.AddSingleton<ICliScraper, GrypeCliScraper>();
    }

    private static void RegisterDocumentationScrapers(IServiceCollection services)
    {
        services.AddSingleton<ICliDocumentationScraper, HelmDocumentationScraper>();
        services.AddSingleton<ICliDocumentationScraper, KubectlDocumentationScraper>();
        services.AddSingleton<ICliDocumentationScraper, DockerDocumentationScraper>();
        services.AddSingleton<ICliDocumentationScraper, AzureCliDocumentationScraper>();
        services.AddSingleton<ICliDocumentationScraper, DotNetCliDocumentationScraper>();
        services.AddSingleton<ICliDocumentationScraper, BrewDocumentationScraper>();
    }

    private static void RegisterGenerators(IServiceCollection services)
    {
        services.AddSingleton<ICodeGenerator, OptionsClassGenerator>();
        services.AddSingleton<ICodeGenerator, EnumGenerator>();
        services.AddSingleton<ICodeGenerator, ServiceInterfaceGenerator>();
        services.AddSingleton<ICodeGenerator, ServiceImplementationGenerator>();
        services.AddSingleton<ICodeGenerator, SubDomainClassGenerator>();
        services.AddSingleton<ICodeGenerator, GlobalOptionsBaseGenerator>();
        services.AddSingleton<ICodeGenerator, DependencyRegistrationGenerator>();
        services.AddSingleton<ICodeGenerator, MarkdownDocumentationGenerator>();
    }

    private static CodeGeneratorOrchestrator CreateOrchestrator(IHost host, bool external)
    {
        if (!external)
        {
            DocumentationExampleCatalog.ValidateRegisteredTools(
                host.Services.GetServices<ICliScraper>()
                    .Select(scraper => scraper.ToolName)
                    .Concat(host.Services.GetServices<ICliDocumentationScraper>()
                        .Select(scraper => scraper.ToolName)));
            return host.Services.GetRequiredService<CodeGeneratorOrchestrator>();
        }

        return new CodeGeneratorOrchestrator(
            cliScrapers: [],
            htmlScrapers: [],
            host.Services.GetServices<ICodeGenerator>(),
            host.Services.GetRequiredService<ILogger<CodeGeneratorOrchestrator>>());
    }

    private static void LogConfiguration(ILogger logger, GeneratorSettings settings)
    {
        logger.LogInformation("Starting CLI Options Generator");
        if (settings.Input is null)
        {
            logger.LogInformation("Tools: {Tools}", settings.Tools);
            logger.LogInformation(
                "CLI-first scraping: {UseCliFirst}",
                settings.UseCliFirst ? "Enabled" : "Disabled");
            logger.LogInformation(
                "Type enhancement: {EnhanceTypes}",
                settings.EnhanceTypes ? "Enabled" : "Disabled");
        }
        else
        {
            logger.LogInformation("External metadata: {Input}", Path.GetFullPath(settings.Input));
        }

        logger.LogInformation("Output directory: {OutputDir}", Path.GetFullPath(settings.OutputDirectory));
        logger.LogInformation(
            "Command coverage change approval: {Approval}",
            settings.ApproveCommandCoverageShrinkage ? "Enabled" : "Disabled");
    }

    private static async Task<GenerationResult> GenerateAsync(
        CodeGeneratorOrchestrator orchestrator,
        GeneratorSettings settings,
        CancellationToken cancellationToken)
    {
        if (settings.Input is null)
        {
            return await orchestrator.GenerateAsync(
                settings.Tools,
                settings.OutputDirectory,
                settings.UseCliFirst,
                settings.ApproveCommandCoverageShrinkage,
                cancellationToken);
        }

        var tool = await ExternalToolDefinitionLoader.LoadAsync(
            settings.Input,
            settings.OutputDirectory,
            cancellationToken);
        return await orchestrator.GenerateFromDefinitionAsync(
            tool,
            settings.OutputDirectory,
            settings.ApproveCommandCoverageShrinkage,
            cancellationToken);
    }

    private static async Task WriteChangeManifestAsync(
        GenerationResult result,
        string? changeManifest,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(changeManifest))
        {
            return;
        }

        var manifestPath = Path.GetFullPath(changeManifest);
        Directory.CreateDirectory(Path.GetDirectoryName(manifestPath)!);
        await File.WriteAllLinesAsync(
            manifestPath,
            result.ChangedPaths.Order(StringComparer.OrdinalIgnoreCase),
            cancellationToken);
    }

    private static int WriteResult(GenerationResult result)
    {
        Console.WriteLine(result.GetSummary());
        if (!result.HasErrors)
        {
            return 0;
        }

        Console.WriteLine("\nErrors:");
        foreach (var error in result.Errors)
        {
            Console.WriteLine($"  - [{error.Source}] {error.Message}");
        }

        return 1;
    }

    private sealed record CommandOptions(
        Option<string> Tools,
        Option<string> OutputDirectory,
        Option<string?> Input,
        Option<bool> UseCliFirst,
        Option<bool> EnhanceTypes,
        Option<bool> ApproveCommandCoverageShrinkage,
        Option<string?> ChangeManifest,
        Option<bool> ListTools,
        Option<bool> Json);

    private sealed record GeneratorSettings(
        string Tools,
        string OutputDirectory,
        string? Input,
        bool UseCliFirst,
        bool EnhanceTypes,
        bool ApproveCommandCoverageShrinkage,
        string? ChangeManifest,
        bool ListTools,
        bool Json);
}
