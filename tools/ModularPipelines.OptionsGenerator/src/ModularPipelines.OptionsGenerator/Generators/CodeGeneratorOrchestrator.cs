using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Base;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Generators;

/// <summary>
/// Orchestrates the scraping and code generation process.
/// Supports both CLI-first scrapers (preferred) and HTML scrapers (fallback).
/// </summary>
public class CodeGeneratorOrchestrator
{
    private readonly IEnumerable<ICliScraper> _cliScrapers;
    private readonly IEnumerable<ICliDocumentationScraper> _htmlScrapers;
    private readonly IEnumerable<ICodeGenerator> _generators;
    private readonly ILogger<CodeGeneratorOrchestrator> _logger;
    private readonly OptionTypeEnhancer? _typeEnhancer;

    public CodeGeneratorOrchestrator(
        IEnumerable<ICliScraper> cliScrapers,
        IEnumerable<ICliDocumentationScraper> htmlScrapers,
        IEnumerable<ICodeGenerator> generators,
        ILogger<CodeGeneratorOrchestrator> logger,
        OptionTypeEnhancer? typeEnhancer = null)
    {
        ArgumentNullException.ThrowIfNull(cliScrapers);
        ArgumentNullException.ThrowIfNull(htmlScrapers);
        ArgumentNullException.ThrowIfNull(generators);
        ArgumentNullException.ThrowIfNull(logger);

        _cliScrapers = cliScrapers;
        _htmlScrapers = htmlScrapers;
        _generators = generators;
        _logger = logger;
        _typeEnhancer = typeEnhancer;
    }

    /// <summary>
    /// Generates code for the specified tools.
    /// </summary>
    /// <param name="toolsToGenerate">Comma-separated list of tools or "all".</param>
    /// <param name="outputDirectory">Output directory path.</param>
    /// <param name="useCliFirst">When true, prefer CLI scrapers over HTML scrapers.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task<GenerationResult> GenerateAsync(
        string toolsToGenerate,
        string outputDirectory,
        bool useCliFirst = true,
        CancellationToken cancellationToken = default)
    {
        var result = new GenerationResult();
        var toolList = ParseToolList(toolsToGenerate);
        var processedTools = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Build a lookup of CLI scrapers by tool name
        var cliScrapersByTool = _cliScrapers.ToDictionary(s => s.ToolName, s => s, StringComparer.OrdinalIgnoreCase);

        // Process each HTML scraper (as the baseline list of tools)
        foreach (var htmlScraper in _htmlScrapers)
        {
            if (!ShouldProcess(htmlScraper.ToolName, toolList))
            {
                _logger.LogInformation("Skipping {Tool}", htmlScraper.ToolName);
                continue;
            }

            if (processedTools.Contains(htmlScraper.ToolName))
            {
                continue;
            }

            _logger.LogInformation("Processing {Tool}...", htmlScraper.ToolName);

            try
            {
                processedTools.Add(htmlScraper.ToolName);
                result.ToolsProcessed.Add(htmlScraper.ToolName);

                // Try CLI scraper first if enabled (streaming)
                if (useCliFirst && cliScrapersByTool.TryGetValue(htmlScraper.ToolName, out var cliScraper))
                {
                    if (await cliScraper.IsAvailableAsync(cancellationToken))
                    {
                        _logger.LogInformation("Using CLI scraper for {Tool}", htmlScraper.ToolName);

                        var toolDefinition = cliScraper.CreateToolDefinition();
                        var allCommands = new List<CliCommandDefinition>();

                        // Collect all commands first
                        await foreach (var command in cliScraper.ScrapeAsync(cancellationToken))
                        {
                            allCommands.Add(command);
                        }

                        _logger.LogInformation("Scraped {Count} commands for {Tool}", allCommands.Count, htmlScraper.ToolName);

                        // Create complete tool definition
                        var completeToolDefinition = new CliToolDefinition
                        {
                            ToolName = toolDefinition.ToolName,
                            NamespacePrefix = toolDefinition.NamespacePrefix,
                            TargetNamespace = toolDefinition.TargetNamespace,
                            OutputDirectory = toolDefinition.OutputDirectory,
                            Commands = allCommands,
                            Errors = []
                        };

                        // Run all generators with complete tool definition
                        foreach (var generator in _generators)
                        {
                            var files = await generator.GenerateAsync(completeToolDefinition, cancellationToken);

                            foreach (var file in files)
                            {
                                var fullPath = Path.Combine(outputDirectory, file.RelativePath);
                                await WriteFileAsync(fullPath, file.Content, cancellationToken);
                                result.FilesGenerated.Add(file.RelativePath);
                            }
                        }

                        _logger.LogInformation("Generated files for {Tool} ({Count} commands, {SubDomainCount} sub-domains)",
                            htmlScraper.ToolName, allCommands.Count, completeToolDefinition.SubDomainGroups.Count);
                        continue;
                    }

                    _logger.LogWarning("{Tool} CLI not available, falling back to HTML scraper", htmlScraper.ToolName);
                }

                // Fall back to HTML scraper (batch mode)
                _logger.LogInformation("Using HTML scraper for {Tool}", htmlScraper.ToolName);
                var htmlToolDefinition = await htmlScraper.ScrapeAsync(cancellationToken);

                if (htmlToolDefinition.Errors.Count > 0)
                {
                    result.Errors.AddRange(htmlToolDefinition.Errors);
                    _logger.LogWarning("Encountered {Count} errors while scraping {Tool}",
                        htmlToolDefinition.Errors.Count, htmlScraper.ToolName);
                }

                _logger.LogInformation("Scraped {Count} commands for {Tool}",
                    htmlToolDefinition.Commands.Count, htmlScraper.ToolName);

                // Enhance types if enhancer is available
                if (_typeEnhancer is not null)
                {
                    _logger.LogInformation("Enhancing types for {Tool} using CLI help...", htmlScraper.ToolName);
                    try
                    {
                        htmlToolDefinition = await _typeEnhancer.EnhanceAsync(htmlToolDefinition, cancellationToken);
                        _logger.LogInformation("Type enhancement complete for {Tool}", htmlScraper.ToolName);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Type enhancement failed for {Tool}, using scraped types", htmlScraper.ToolName);
                    }
                }

                // Generate code using all generators
                foreach (var generator in _generators)
                {
                    var files = await generator.GenerateAsync(htmlToolDefinition, cancellationToken);

                    foreach (var file in files)
                    {
                        var fullPath = Path.Combine(outputDirectory, file.RelativePath);
                        await WriteFileAsync(fullPath, file.Content, cancellationToken);
                        result.FilesGenerated.Add(file.RelativePath);
                    }
                }

                _logger.LogInformation("Generated files for {Tool}", htmlScraper.ToolName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process {Tool}", htmlScraper.ToolName);
                result.Errors.Add(new ScrapingError
                {
                    Source = htmlScraper.ToolName,
                    Message = ex.Message,
                    Exception = ex
                });
            }
        }

        // Process CLI-only scrapers (tools that have no HTML scraper)
        if (useCliFirst)
        {
            foreach (var cliScraper in _cliScrapers)
            {
                if (processedTools.Contains(cliScraper.ToolName))
                {
                    continue;
                }

                if (!ShouldProcess(cliScraper.ToolName, toolList))
                {
                    _logger.LogInformation("Skipping CLI-only tool {Tool}", cliScraper.ToolName);
                    continue;
                }

                _logger.LogInformation("Processing CLI-only tool {Tool}...", cliScraper.ToolName);

                try
                {
                    if (!await cliScraper.IsAvailableAsync(cancellationToken))
                    {
                        _logger.LogWarning("{Tool} CLI not available, skipping", cliScraper.ToolName);
                        continue;
                    }

                    var toolDefinition = cliScraper.CreateToolDefinition();
                    var allCommands = new List<CliCommandDefinition>();

                    processedTools.Add(cliScraper.ToolName);
                    result.ToolsProcessed.Add(cliScraper.ToolName);

                    // Collect all commands first
                    await foreach (var command in cliScraper.ScrapeAsync(cancellationToken))
                    {
                        allCommands.Add(command);
                    }

                    _logger.LogInformation("Scraped {Count} commands for {Tool}", allCommands.Count, cliScraper.ToolName);

                    // Create complete tool definition
                    var completeToolDefinition = new CliToolDefinition
                    {
                        ToolName = toolDefinition.ToolName,
                        NamespacePrefix = toolDefinition.NamespacePrefix,
                        TargetNamespace = toolDefinition.TargetNamespace,
                        OutputDirectory = toolDefinition.OutputDirectory,
                        Commands = allCommands,
                        Errors = []
                    };

                    // Run all generators with complete tool definition
                    foreach (var generator in _generators)
                    {
                        var files = await generator.GenerateAsync(completeToolDefinition, cancellationToken);

                        foreach (var file in files)
                        {
                            var fullPath = Path.Combine(outputDirectory, file.RelativePath);
                            await WriteFileAsync(fullPath, file.Content, cancellationToken);
                            result.FilesGenerated.Add(file.RelativePath);
                        }
                    }

                    _logger.LogInformation("Generated files for {Tool} ({Count} commands, {SubDomainCount} sub-domains)",
                        cliScraper.ToolName, allCommands.Count, completeToolDefinition.SubDomainGroups.Count);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to process CLI-only tool {Tool}", cliScraper.ToolName);
                    result.Errors.Add(new ScrapingError
                    {
                        Source = cliScraper.ToolName,
                        Message = ex.Message,
                        Exception = ex
                    });
                }
            }
        }

        return result;
    }

    private static List<string> ParseToolList(string tools)
    {
        if (string.Equals(tools, "all", StringComparison.OrdinalIgnoreCase))
        {
            return [];
        }

        return tools.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(t => t.Trim().ToLowerInvariant())
            .ToList();
    }

    private static bool ShouldProcess(string toolName, List<string> toolList)
    {
        if (toolList.Count == 0)
        {
            return true; // Process all
        }

        return toolList.Contains(toolName.ToLowerInvariant());
    }

    private static async Task WriteFileAsync(string path, string content, CancellationToken cancellationToken)
    {
        var directory = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await File.WriteAllTextAsync(path, content, cancellationToken);
    }
}

/// <summary>
/// Result of the code generation process.
/// </summary>
public class GenerationResult
{
    public List<string> ToolsProcessed { get; } = [];
    public List<string> FilesGenerated { get; } = [];
    public List<ScrapingError> Errors { get; } = [];

    public bool HasErrors => Errors.Count > 0;

    public string GetSummary()
    {
        return $"""
            Generation Summary:
            - Tools processed: {ToolsProcessed.Count} ({string.Join(", ", ToolsProcessed)})
            - Files generated: {FilesGenerated.Count}
            - Errors: {Errors.Count}
            """;
    }
}
