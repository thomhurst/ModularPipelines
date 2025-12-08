using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Base;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
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

    /// <summary>
    /// Scrapes CLI documentation using a CLI-first scraper and applies type enhancement.
    /// CLI-first scrapers parse --help output directly, which is the authoritative source.
    /// </summary>
    /// <param name="scraper">The CLI scraper to use.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The enhanced tool definition.</returns>
    public async Task<CliToolDefinition> ScrapeAndEnhanceAsync(
        ICliScraper scraper,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(scraper);

        _logger.LogInformation("Starting CLI-first scrape for {Tool}", scraper.ToolName);

        // Check if tool is available
        if (!await scraper.IsAvailableAsync(cancellationToken))
        {
            _logger.LogWarning("{Tool} is not available on this system, skipping", scraper.ToolName);
            return new CliToolDefinition
            {
                ToolName = scraper.ToolName,
                NamespacePrefix = scraper.NamespacePrefix,
                TargetNamespace = scraper.TargetNamespace,
                OutputDirectory = scraper.OutputDirectory,
                Commands = [],
                Errors = [new ScrapingError
                {
                    Source = scraper.ToolName,
                    Message = $"{scraper.ToolName} is not available on this system"
                }]
            };
        }

        // Step 1: Scrape using CLI help (streaming - collect all commands)
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync(cancellationToken))
        {
            commands.Add(command);
        }

        var toolDefinition = new CliToolDefinition
        {
            ToolName = scraper.ToolName,
            NamespacePrefix = scraper.NamespacePrefix,
            TargetNamespace = scraper.TargetNamespace,
            OutputDirectory = scraper.OutputDirectory,
            Commands = commands,
            Errors = []
        };

        _logger.LogInformation(
            "Scraped {CommandCount} commands for {Tool} via CLI",
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
    /// Scrapes multiple CLI tools using CLI-first scrapers in parallel.
    /// </summary>
    /// <param name="scrapers">The CLI scrapers to run.</param>
    /// <param name="maxParallelism">Maximum number of concurrent scraping operations.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of enhanced tool definitions.</returns>
    public async Task<IReadOnlyList<CliToolDefinition>> ScrapeAllCliAsync(
        IEnumerable<ICliScraper> scrapers,
        int maxParallelism = 3,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(scrapers);

        var scraperList = scrapers.ToList();
        _logger.LogInformation("Starting parallel CLI-first scrape of {Count} tools", scraperList.Count);

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
            "Completed CLI-first scraping {Count} tools with {TotalCommands} total commands",
            results.Length,
            results.Sum(r => r.Commands.Count));

        return results;
    }
}
