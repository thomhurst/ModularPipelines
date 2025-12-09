using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Scrapers;
using ModularPipelines.OptionsGenerator.Scrapers.Base;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

var toolsOption = new Option<string>("--tools", "-t")
{
    Description = "Comma-separated list of tools to generate, or 'all'",
    DefaultValueFactory = _ => "all"
};

var outputOption = new Option<string>("--output-dir", "-o")
{
    Description = "Output directory (repository root)",
    DefaultValueFactory = _ => "."
};

var useCliFirstOption = new Option<bool>("--use-cli-first")
{
    Description = "Use CLI --help parsing instead of HTML scraping (requires CLI tools to be installed). Recommended for Cobra CLIs (helm, docker, kubectl).",
    DefaultValueFactory = _ => true
};

var enhanceTypesOption = new Option<bool>("--enhance-types")
{
    Description = "Use CLI --help output to enhance type detection after scraping",
    DefaultValueFactory = _ => true
};

var rootCommand = new RootCommand("ModularPipelines CLI Options Generator");
rootCommand.Options.Add(toolsOption);
rootCommand.Options.Add(outputOption);
rootCommand.Options.Add(useCliFirstOption);
rootCommand.Options.Add(enhanceTypesOption);

rootCommand.SetAction(async (parseResult, cancellationToken) =>
{
    var tools = parseResult.GetValue(toolsOption) ?? "all";
    var outputDir = parseResult.GetValue(outputOption) ?? ".";
    var useCliFirst = parseResult.GetValue(useCliFirstOption);
    var enhanceTypes = parseResult.GetValue(enhanceTypesOption);

    var builder = Host.CreateApplicationBuilder();

    // Configure logging
    builder.Logging.AddConsole();
    builder.Logging.SetMinimumLevel(LogLevel.Information);

    // Configure HTTP client (for HTML scrapers as fallback)
    builder.Services.AddHttpClient();

    // Register CLI command executor with resilience patterns (retry + circuit breaker)
    builder.Services.AddSingleton<ProcessCliCommandExecutor>();
    builder.Services.AddSingleton<ICliCommandExecutor>(sp =>
    {
        var inner = sp.GetRequiredService<ProcessCliCommandExecutor>();
        var logger = sp.GetRequiredService<ILogger<ResilientCliCommandExecutor>>();
        return new ResilientCliCommandExecutor(inner, logger);
    });

    // Register help text cache with TTL and size limits
    builder.Services.AddSingleton<IHelpTextCache, HelpTextCache>();

    // Register CLI-first scrapers for Cobra-based CLIs
    builder.Services.AddSingleton<ICliScraper, HelmCliScraper>();
    builder.Services.AddSingleton<ICliScraper, DockerCliScraper>();
    builder.Services.AddSingleton<ICliScraper, KubectlCliScraper>();
    builder.Services.AddSingleton<ICliScraper, GcloudCliScraper>();
    builder.Services.AddSingleton<ICliScraper, GitCliScraper>();

    // Register additional CLI scrapers
    builder.Services.AddSingleton<ICliScraper, TerraformCliScraper>();
    builder.Services.AddSingleton<ICliScraper, WinGetCliScraper>();
    builder.Services.AddSingleton<ICliScraper, ChocolateyCliScraper>();
    builder.Services.AddSingleton<ICliScraper, YarnCliScraper>();
    builder.Services.AddSingleton<ICliScraper, AwsCliScraper>();
    builder.Services.AddSingleton<ICliScraper, AzCliScraper>();
    builder.Services.AddSingleton<ICliScraper, DotNetCliScraper>();

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

        return 1;
    }

    return 0;
});

var parseResult = rootCommand.Parse(args);
return await parseResult.InvokeAsync();
