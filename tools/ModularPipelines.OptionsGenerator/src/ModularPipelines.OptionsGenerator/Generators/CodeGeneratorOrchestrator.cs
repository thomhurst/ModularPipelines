using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Base;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Generators;

/// <summary>
/// Orchestrates the scraping and code generation process.
/// </summary>
public class CodeGeneratorOrchestrator
{
    private readonly IEnumerable<ICliDocumentationScraper> _scrapers;
    private readonly IEnumerable<ICodeGenerator> _generators;
    private readonly ILogger<CodeGeneratorOrchestrator> _logger;
    private readonly OptionTypeEnhancer? _typeEnhancer;

    public CodeGeneratorOrchestrator(
        IEnumerable<ICliDocumentationScraper> scrapers,
        IEnumerable<ICodeGenerator> generators,
        ILogger<CodeGeneratorOrchestrator> logger,
        OptionTypeEnhancer? typeEnhancer = null)
    {
        ArgumentNullException.ThrowIfNull(scrapers);
        ArgumentNullException.ThrowIfNull(generators);
        ArgumentNullException.ThrowIfNull(logger);

        _scrapers = scrapers;
        _generators = generators;
        _logger = logger;
        _typeEnhancer = typeEnhancer;
    }

    /// <summary>
    /// Generates code for the specified tools.
    /// </summary>
    public async Task<GenerationResult> GenerateAsync(
        string toolsToGenerate,
        string outputDirectory,
        CancellationToken cancellationToken = default)
    {
        var result = new GenerationResult();
        var toolList = ParseToolList(toolsToGenerate);

        foreach (var scraper in _scrapers)
        {
            if (!ShouldProcess(scraper.ToolName, toolList))
            {
                _logger.LogInformation("Skipping {Tool}", scraper.ToolName);
                continue;
            }

            _logger.LogInformation("Processing {Tool}...", scraper.ToolName);

            try
            {
                var toolDefinition = await scraper.ScrapeAsync(cancellationToken);
                result.ToolsProcessed.Add(scraper.ToolName);

                if (toolDefinition.Errors.Count > 0)
                {
                    result.Errors.AddRange(toolDefinition.Errors);
                    _logger.LogWarning("Encountered {Count} errors while scraping {Tool}",
                        toolDefinition.Errors.Count, scraper.ToolName);
                }

                _logger.LogInformation("Scraped {Count} commands for {Tool}",
                    toolDefinition.Commands.Count, scraper.ToolName);

                // Enhance types if enhancer is available
                if (_typeEnhancer is not null)
                {
                    _logger.LogInformation("Enhancing types for {Tool} using CLI help...", scraper.ToolName);
                    try
                    {
                        toolDefinition = await _typeEnhancer.EnhanceAsync(toolDefinition, cancellationToken);
                        _logger.LogInformation("Type enhancement complete for {Tool}", scraper.ToolName);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Type enhancement failed for {Tool}, using scraped types", scraper.ToolName);
                    }
                }

                // Generate code using all generators
                foreach (var generator in _generators)
                {
                    var files = await generator.GenerateAsync(toolDefinition, cancellationToken);

                    foreach (var file in files)
                    {
                        var fullPath = Path.Combine(outputDirectory, file.RelativePath);
                        await WriteFileAsync(fullPath, file.Content, cancellationToken);
                        result.FilesGenerated.Add(file.RelativePath);
                    }
                }

                _logger.LogInformation("Generated files for {Tool}", scraper.ToolName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process {Tool}", scraper.ToolName);
                result.Errors.Add(new ScrapingError
                {
                    Source = scraper.ToolName,
                    Message = ex.Message,
                    Exception = ex
                });
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
