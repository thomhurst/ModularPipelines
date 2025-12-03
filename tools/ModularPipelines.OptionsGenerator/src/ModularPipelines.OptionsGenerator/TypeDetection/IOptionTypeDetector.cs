namespace ModularPipelines.OptionsGenerator.TypeDetection;

/// <summary>
/// Interface for detecting the type of a CLI option.
/// Implementations can use various strategies (help text, manual overrides, web scraping).
/// </summary>
public interface IOptionTypeDetector
{
    /// <summary>
    /// Priority of this detector. Lower values are checked first.
    /// Suggested ranges:
    /// - 0-99: Manual overrides (highest priority)
    /// - 100-199: Help text parsing (most reliable automated)
    /// - 200-299: Web scraping fallback
    /// - 300+: Heuristic guessing
    /// </summary>
    int Priority { get; }

    /// <summary>
    /// Name of this detector for logging/debugging.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Determines if this detector can handle options for the given tool.
    /// </summary>
    /// <param name="toolName">The CLI tool name (e.g., "docker", "kubectl", "az").</param>
    /// <returns>True if this detector should be used for the tool.</returns>
    bool CanHandle(string toolName);

    /// <summary>
    /// Attempts to detect the type of the given option.
    /// </summary>
    /// <param name="context">Context containing option information and caching.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Detection result with type and confidence, or Unknown if cannot determine.</returns>
    Task<OptionTypeDetectionResult> DetectTypeAsync(
        OptionDetectionContext context,
        CancellationToken cancellationToken = default);
}
