using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Scrapers;
using ModularPipelines.OptionsGenerator.Scrapers.Base;
using ModularPipelines.OptionsGenerator.TypeDetection;

var toolsOption = new Option<string>(
    name: "--tools",
    description: "Comma-separated list of tools to generate, or 'all'",
    getDefaultValue: () => "all");

var outputOption = new Option<string>(
    name: "--output-dir",
    description: "Output directory (repository root)",
    getDefaultValue: () => ".");

var enhanceTypesOption = new Option<bool>(
    name: "--enhance-types",
    description: "Use CLI --help output to enhance type detection (requires CLI tools to be installed)",
    getDefaultValue: () => false);

var rootCommand = new RootCommand("ModularPipelines CLI Options Generator")
{
    toolsOption,
    outputOption,
    enhanceTypesOption
};

rootCommand.SetHandler(async (tools, outputDir, enhanceTypes) =>
{
    var builder = Host.CreateApplicationBuilder();

    // Configure logging
    builder.Logging.AddConsole();
    builder.Logging.SetMinimumLevel(LogLevel.Information);

    // Configure HTTP client
    builder.Services.AddHttpClient();

    // Register scrapers
    builder.Services.AddSingleton<ICliDocumentationScraper, HelmDocumentationScraper>();
    builder.Services.AddSingleton<ICliDocumentationScraper, KubectlDocumentationScraper>();
    builder.Services.AddSingleton<ICliDocumentationScraper, DockerDocumentationScraper>();
    builder.Services.AddSingleton<ICliDocumentationScraper, AzureCliDocumentationScraper>();
    builder.Services.AddSingleton<ICliDocumentationScraper, DotNetCliDocumentationScraper>();
    // builder.Services.AddSingleton<ICliDocumentationScraper, NpmDocumentationScraper>(); // Requires Playwright

    // Register generators
    builder.Services.AddSingleton<ICodeGenerator, OptionsClassGenerator>();
    builder.Services.AddSingleton<ICodeGenerator, EnumGenerator>();
    builder.Services.AddSingleton<ICodeGenerator, ServiceInterfaceGenerator>();
    builder.Services.AddSingleton<ICodeGenerator, SubDomainClassGenerator>();

    // Register type enhancer if enabled
    if (enhanceTypes)
    {
        builder.Services.AddSingleton<ICliCommandExecutor, ProcessCliCommandExecutor>();
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
    logger.LogInformation("Type enhancement: {EnhanceTypes}", enhanceTypes ? "Enabled" : "Disabled");

    var result = await orchestrator.GenerateAsync(tools, outputDir);

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

}, toolsOption, outputOption, enhanceTypesOption);

await rootCommand.InvokeAsync(args);
