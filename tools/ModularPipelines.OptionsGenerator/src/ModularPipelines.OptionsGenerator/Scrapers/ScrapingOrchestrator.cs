using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Base;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers;

/// <summary>
/// Orchestrates the scraping workflow by combining scraping with type enhancement.
/// This separates concerns: scrapers focus on HTML parsing, enhancers focus on type detection.
/// </summary>
public class ScrapingOrchestrator
{
    private readonly OptionTypeEnhancer? _enhancer;
    private readonly ILogger<ScrapingOrchestrator> _logger;

    /// <summary>
    /// Creates a new orchestrator with optional type enhancement.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <param name="enhancer">Optional type enhancer for improved type detection.</param>
    public ScrapingOrchestrator(ILogger<ScrapingOrchestrator> logger, OptionTypeEnhancer? enhancer = null)
    {
        ArgumentNullException.ThrowIfNull(logger);

        _logger = logger;
        _enhancer = enhancer;
    }

    /// <summary>
    /// Creates an orchestrator with the default type enhancement pipeline.
    /// </summary>
    public static ScrapingOrchestrator CreateWithDefaultEnhancer(
        ILoggerFactory loggerFactory,
        string? overridesDirectory = null)
    {
        ArgumentNullException.ThrowIfNull(loggerFactory);

        var enhancer = OptionTypeEnhancer.CreateDefault(loggerFactory, overridesDirectory);
        return new ScrapingOrchestrator(
            loggerFactory.CreateLogger<ScrapingOrchestrator>(),
            enhancer);
    }

    /// <summary>
    /// Creates an orchestrator without type enhancement (basic scraping only).
    /// </summary>
    public static ScrapingOrchestrator CreateWithoutEnhancer(ILoggerFactory loggerFactory)
    {
        ArgumentNullException.ThrowIfNull(loggerFactory);

        return new ScrapingOrchestrator(
            loggerFactory.CreateLogger<ScrapingOrchestrator>());
    }

    /// <summary>
    /// Scrapes CLI documentation and applies type enhancement.
    /// </summary>
    /// <param name="scraper">The scraper to use for fetching documentation.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The enhanced tool definition.</returns>
    public async Task<CliToolDefinition> ScrapeAndEnhanceAsync(
        ICliDocumentationScraper scraper,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(scraper);

        _logger.LogInformation("Starting scrape for {Tool}", scraper.ToolName);

        // Step 1: Scrape the documentation
        var toolDefinition = await scraper.ScrapeAsync(cancellationToken);

        _logger.LogInformation(
            "Scraped {CommandCount} commands for {Tool}",
            toolDefinition.Commands.Count,
            scraper.ToolName);

        // Step 2: Apply type enhancement if available
        if (_enhancer is not null)
        {
            _logger.LogInformation("Applying type enhancement for {Tool}", scraper.ToolName);
            toolDefinition = await _enhancer.EnhanceAsync(toolDefinition, cancellationToken);
            _logger.LogInformation("Type enhancement complete for {Tool}", scraper.ToolName);
        }
        else
        {
            _logger.LogDebug("Type enhancement not configured, using scraped types as-is");
        }

        // Log any errors encountered during scraping
        if (toolDefinition.Errors.Count > 0)
        {
            _logger.LogWarning(
                "Scraping completed with {ErrorCount} errors for {Tool}",
                toolDefinition.Errors.Count,
                scraper.ToolName);

            foreach (var error in toolDefinition.Errors.Take(10))
            {
                _logger.LogWarning("  - {Source}: {Message}", error.Source, error.Message);
            }
        }

        return toolDefinition;
    }

    /// <summary>
    /// Scrapes multiple CLI tools in parallel and applies type enhancement.
    /// </summary>
    /// <param name="scrapers">The scrapers to run.</param>
    /// <param name="maxParallelism">Maximum number of concurrent scraping operations.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of enhanced tool definitions.</returns>
    public async Task<IReadOnlyList<CliToolDefinition>> ScrapeAllAsync(
        IEnumerable<ICliDocumentationScraper> scrapers,
        int maxParallelism = 3,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(scrapers);

        var scraperList = scrapers.ToList();
        _logger.LogInformation("Starting parallel scrape of {Count} tools", scraperList.Count);

        var semaphore = new SemaphoreSlim(maxParallelism);
        var tasks = scraperList.Select(async scraper =>
        {
            await semaphore.WaitAsync(cancellationToken);
            try
            {
                return await ScrapeAndEnhanceAsync(scraper, cancellationToken);
            }
            finally
            {
                semaphore.Release();
            }
        });

        var results = await Task.WhenAll(tasks);

        _logger.LogInformation(
            "Completed scraping {Count} tools with {TotalCommands} total commands",
            results.Length,
            results.Sum(r => r.Commands.Count));

        return results;
    }
}
