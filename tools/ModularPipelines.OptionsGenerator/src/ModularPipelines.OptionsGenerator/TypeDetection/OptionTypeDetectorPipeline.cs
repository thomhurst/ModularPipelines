using Microsoft.Extensions.Logging;

namespace ModularPipelines.OptionsGenerator.TypeDetection;

/// <summary>
/// Orchestrates multiple type detectors in priority order.
/// Returns the first result with sufficient confidence.
/// </summary>
public class OptionTypeDetectorPipeline
{
    private readonly IReadOnlyList<IOptionTypeDetector> _detectors;
    private readonly ILogger<OptionTypeDetectorPipeline> _logger;
    private readonly int _minimumConfidence;

    /// <summary>
    /// Creates a new detector pipeline.
    /// </summary>
    /// <param name="detectors">Available detectors (will be sorted by priority).</param>
    /// <param name="logger">Logger for pipeline operations.</param>
    /// <param name="minimumConfidence">Minimum confidence to accept a result (default: 60).</param>
    public OptionTypeDetectorPipeline(
        IEnumerable<IOptionTypeDetector> detectors,
        ILogger<OptionTypeDetectorPipeline> logger,
        int minimumConfidence = 60)
    {
        ArgumentNullException.ThrowIfNull(detectors);
        ArgumentNullException.ThrowIfNull(logger);

        _detectors = detectors.OrderBy(d => d.Priority).ToList();
        _logger = logger;
        _minimumConfidence = minimumConfidence;
    }

    /// <summary>
    /// Detects the type of an option using all available detectors.
    /// </summary>
    /// <param name="context">Option detection context.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Best detection result, or Unknown if no detector succeeded.</returns>
    public async Task<OptionTypeDetectionResult> DetectTypeAsync(
        OptionDetectionContext context,
        CancellationToken cancellationToken = default)
    {
        OptionTypeDetectionResult? bestResult = null;

        foreach (var detector in _detectors)
        {
            if (!detector.CanHandle(context.ToolName))
            {
                continue;
            }

            try
            {
                var result = await detector.DetectTypeAsync(context, cancellationToken);

                if (result.Type == CliOptionType.Unknown)
                {
                    continue;
                }

                _logger.LogDebug(
                    "Detector {Detector} returned {Type} with confidence {Confidence} for {Option}",
                    detector.Name, result.Type, result.Confidence, context.OptionName);

                // If confidence is high enough, return immediately
                if (result.Confidence >= _minimumConfidence)
                {
                    return result;
                }

                // Track best result so far
                if (bestResult is null || result.Confidence > bestResult.Confidence)
                {
                    bestResult = result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex,
                    "Detector {Detector} failed for option {Option}",
                    detector.Name, context.OptionName);
            }
        }

        // Return best result even if below minimum confidence
        if (bestResult is not null)
        {
            _logger.LogDebug(
                "Using best available result: {Type} with confidence {Confidence} for {Option}",
                bestResult.Type, bestResult.Confidence, context.OptionName);
            return bestResult;
        }

        _logger.LogDebug("No detector could determine type for {Option}", context.OptionName);
        return OptionTypeDetectionResult.Unknown("Pipeline");
    }

    /// <summary>
    /// Creates a default pipeline with all standard detectors.
    /// </summary>
    public static OptionTypeDetectorPipeline CreateDefault(
        ICliCommandExecutor executor,
        ILoggerFactory loggerFactory,
        string? overridesDirectory = null)
    {
        var detectors = new List<IOptionTypeDetector>
        {
            new ManualOverrideDetector(
                loggerFactory.CreateLogger<ManualOverrideDetector>(),
                overridesDirectory),
            new CobraHelpTypeDetector(
                executor,
                loggerFactory.CreateLogger<CobraHelpTypeDetector>()),
            new DotNetHelpTypeDetector(
                executor,
                loggerFactory.CreateLogger<DotNetHelpTypeDetector>()),
            new AzureCliHelpTypeDetector(
                executor,
                loggerFactory.CreateLogger<AzureCliHelpTypeDetector>()),
            new HeuristicTypeDetector(
                loggerFactory.CreateLogger<HeuristicTypeDetector>())
        };

        return new OptionTypeDetectorPipeline(
            detectors,
            loggerFactory.CreateLogger<OptionTypeDetectorPipeline>());
    }
}
