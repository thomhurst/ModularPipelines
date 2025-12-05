using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Scrapers;
using ModularPipelines.OptionsGenerator.Scrapers.Base;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

var toolsOption = new Option<string>(
    name: "--tools",
    description: "Comma-separated list of tools to generate, or 'all'",
    getDefaultValue: () => "all");

var outputOption = new Option<string>(
    name: "--output-dir",
    description: "Output directory (repository root)",
    getDefaultValue: () => ".");

var useCliFirstOption = new Option<bool>(
    name: "--use-cli-first",
    description: "Use CLI --help parsing instead of HTML scraping (requires CLI tools to be installed). Recommended for Cobra CLIs (helm, docker, kubectl).",
    getDefaultValue: () => true);

var enhanceTypesOption = new Option<bool>(
    name: "--enhance-types",
    description: "Use CLI --help output to enhance type detection after scraping",
    getDefaultValue: () => true);

var rootCommand = new RootCommand("ModularPipelines CLI Options Generator")
{
    toolsOption,
    outputOption,
    useCliFirstOption,
    enhanceTypesOption
};

rootCommand.SetHandler(async (tools, outputDir, useCliFirst, enhanceTypes) =>
{
    var builder = Host.CreateApplicationBuilder();

    // Configure logging
    builder.Logging.AddConsole();
    builder.Logging.SetMinimumLevel(LogLevel.Information);

    // Configure HTTP client (for HTML scrapers as fallback)
    builder.Services.AddHttpClient();

    // Register CLI command executor (needed for CLI scrapers and type enhancement)
    builder.Services.AddSingleton<ICliCommandExecutor, ProcessCliCommandExecutor>();

    // Register CLI-first scrapers for Cobra-based CLIs
    builder.Services.AddSingleton<ICliScraper, HelmCliScraper>();
    builder.Services.AddSingleton<ICliScraper, DockerCliScraper>();
    builder.Services.AddSingleton<ICliScraper, KubectlCliScraper>();

    // Register HTML scrapers (used as fallback or for non-Cobra CLIs)
    builder.Services.AddSingleton<ICliDocumentationScraper, HelmDocumentationScraper>();
    builder.Services.AddSingleton<ICliDocumentationScraper, KubectlDocumentationScraper>();
    builder.Services.AddSingleton<ICliDocumentationScraper, DockerDocumentationScraper>();
    builder.Services.AddSingleton<ICliDocumentationScraper, AzureCliDocumentationScraper>();
    builder.Services.AddSingleton<ICliDocumentationScraper, DotNetCliDocumentationScraper>();

    // Register generators
    builder.Services.AddSingleton<ICodeGenerator, OptionsClassGenerator>();
    builder.Services.AddSingleton<ICodeGenerator, EnumGenerator>();
    builder.Services.AddSingleton<ICodeGenerator, ServiceInterfaceGenerator>();
    builder.Services.AddSingleton<ICodeGenerator, ServiceImplementationGenerator>();
    builder.Services.AddSingleton<ICodeGenerator, SubDomainClassGenerator>();
    builder.Services.AddSingleton<ICodeGenerator, GlobalOptionsBaseGenerator>();
    builder.Services.AddSingleton<ICodeGenerator, DependencyRegistrationGenerator>();

    // Register type enhancer
    if (enhanceTypes)
    {
        builder.Services.AddSingleton(sp =>
        {
            var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
            return OptionTypeEnhancer.CreateDefault(loggerFactory);
        });
    }

    // Register orchestrator
    builder.Services.AddSingleton<CodeGeneratorOrchestrator>();

    var host = builder.Build();

    var orchestrator = host.Services.GetRequiredService<CodeGeneratorOrchestrator>();
    var logger = host.Services.GetRequiredService<ILogger<Program>>();

    logger.LogInformation("Starting CLI Options Generator");
    logger.LogInformation("Tools: {Tools}", tools);
    logger.LogInformation("Output directory: {OutputDir}", Path.GetFullPath(outputDir));
    logger.LogInformation("CLI-first scraping: {UseCliFirst}", useCliFirst ? "Enabled" : "Disabled");
    logger.LogInformation("Type enhancement: {EnhanceTypes}", enhanceTypes ? "Enabled" : "Disabled");

    var result = await orchestrator.GenerateAsync(tools, outputDir, useCliFirst);

    Console.WriteLine(result.GetSummary());

    if (result.HasErrors)
    {
        Console.WriteLine("\nErrors:");
        foreach (var error in result.Errors)
        {
            Console.WriteLine($"  - [{error.Source}] {error.Message}");
        }

        // Exit with error code but don't fail completely (continue on failure)
        Environment.ExitCode = 1;
    }
    else
    {
        Environment.ExitCode = 0;
    }

}, toolsOption, outputOption, useCliFirstOption, enhanceTypesOption);

await rootCommand.InvokeAsync(args);
