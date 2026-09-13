using Kevlar;
using Microsoft.Extensions.Logging;

namespace ModularPipelines.OptionsGenerator.TypeDetection;

/// <summary>
/// Decorator that adds resilience patterns (retry and circuit breaker) to CLI command execution.
/// Uses Kevlar for:
/// - Exponential backoff retry (3 attempts) for transient failures
/// - Circuit breaker (5 failures opens circuit for 30 seconds) to prevent cascading failures
/// </summary>
public sealed class ResilientCliCommandExecutor : ICliCommandExecutor
{
    private readonly ICliCommandExecutor _inner;
    private readonly ILogger<ResilientCliCommandExecutor> _logger;
    private readonly Shield<CliCommandResult> _shield;

    /// <summary>
    /// Default retry count for transient failures.
    /// </summary>
    public const int DefaultMaxRetries = 3;

    /// <summary>
    /// Default base delay for exponential backoff.
    /// </summary>
    public static readonly TimeSpan DefaultBaseDelay = TimeSpan.FromMilliseconds(200);

    /// <summary>
    /// Default circuit breaker failure threshold.
    /// </summary>
    public const int DefaultFailureThreshold = 5;

    /// <summary>
    /// Default circuit breaker break duration.
    /// </summary>
    public static readonly TimeSpan DefaultBreakDuration = TimeSpan.FromSeconds(30);

    public ResilientCliCommandExecutor(
        ICliCommandExecutor inner,
        ILogger<ResilientCliCommandExecutor> logger)
        : this(inner, logger, DefaultMaxRetries, DefaultBaseDelay, DefaultFailureThreshold, DefaultBreakDuration)
    {
    }

    public ResilientCliCommandExecutor(
        ICliCommandExecutor inner,
        ILogger<ResilientCliCommandExecutor> logger,
        int maxRetries,
        TimeSpan baseDelay,
        int circuitBreakerThreshold,
        TimeSpan circuitBreakerDuration)
    {
        ArgumentNullException.ThrowIfNull(inner);
        ArgumentNullException.ThrowIfNull(logger);

        _inner = inner;
        _logger = logger;

        _shield = Shield.For<CliCommandResult>()
            .WhenResult(IsTransientFailure)
            .CircuitBreaker(options =>
            {
                options.ConsecutiveFailures = circuitBreakerThreshold;
                options.BreakDuration = circuitBreakerDuration;
                options.OnStateChanged = stateChangedEvent =>
                {
                    switch (stateChangedEvent.To)
                    {
                        case CircuitState.Open:
                            _logger.LogError(
                                "Circuit breaker OPENED - CLI commands are failing. Will retry after {Duration}s",
                                circuitBreakerDuration.TotalSeconds);
                            break;
                        case CircuitState.Closed:
                            _logger.LogInformation("Circuit breaker CLOSED - CLI commands are healthy again");
                            break;
                        case CircuitState.HalfOpen:
                            _logger.LogInformation("Circuit breaker HALF-OPEN - Testing if CLI commands are healthy...");
                            break;
                    }

                    return ValueTask.CompletedTask;
                };
            })
            .Retry(options =>
            {
                options.MaxRetries = maxRetries;
                options.Backoff = Backoff.Exponential(baseDelay, jitter: Jitter.None);
                options.OnRetry = retryEvent =>
                {
                    _logger.LogWarning(
                        "CLI command failed (attempt {Attempt}/{MaxAttempts}), retrying in {Delay}ms...",
                        retryEvent.AttemptNumber,
                        maxRetries,
                        retryEvent.Delay.TotalMilliseconds);

                    return ValueTask.CompletedTask;
                };
            });
    }

    public async Task<CliCommandResult> ExecuteAsync(
        string command,
        string arguments,
        CancellationToken cancellationToken = default,
        string? workingDirectory = null)
    {
        try
        {
            var result = await _shield.ExecuteAsync(
                    async token => await _inner.ExecuteAsync(command, arguments, token, workingDirectory).ConfigureAwait(false),
                    cancellationToken)
                .ConfigureAwait(false);

            // Legacy executors use -1 for system failures without setting the newer
            // outcome flags. Their final diagnostics must not be parsed as CLI help.
            return result.ExitCode == -1 && !result.Unavailable
                ? new CliCommandResult
                {
                    ExitCode = result.ExitCode,
                    StandardOutput = result.StandardOutput,
                    StandardError = result.StandardError,
                    ExecutionFailed = true,
                }
                : result;
        }
        catch (CircuitOpenException ex)
        {
            _logger.LogError("Circuit breaker is open - CLI execution rejected: {Command} {Arguments}", command, arguments);
            return new CliCommandResult
            {
                StandardOutput = string.Empty,
                StandardError = $"Circuit breaker open: {ex.Message}",
                ExitCode = -2, // Special exit code for circuit breaker rejection
                CircuitOpen = true,
            };
        }
    }

    public async Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default)
    {
        // Availability check doesn't use resilience patterns - we want immediate feedback
        return await _inner.IsAvailableAsync(command, cancellationToken).ConfigureAwait(false);
    }

    public Task<bool> IsAvailableAsync(
        string command,
        string arguments,
        CancellationToken cancellationToken = default)
    {
        // Availability check doesn't use resilience patterns - we want immediate feedback
        return _inner.IsAvailableAsync(command, arguments, cancellationToken);
    }

    /// <summary>
    /// Determines if a failure is transient and should trigger retry.
    /// Explicit timeouts and launch failures participate regardless of the launcher's exit code.
    /// Legacy executors can still report system failures with exit code -1.
    /// </summary>
    private static bool IsTransientFailure(CliCommandResult result)
    {
        if (result.CircuitOpen)
        {
            return false;
        }

        if (result.TimedOut)
        {
            return true;
        }

        // Ordinary tool errors remain final; launch failures are system-level errors.
        if (!result.ExecutionFailed && result.ExitCode != -1)
        {
            return false;
        }

        // Missing executables cannot recover through retries.
        var stderr = result.StandardError ?? string.Empty;
        return !stderr.Contains("not found", StringComparison.OrdinalIgnoreCase)
               && !stderr.Contains("not recognized", StringComparison.OrdinalIgnoreCase)
               && !stderr.Contains("cannot find", StringComparison.OrdinalIgnoreCase)
               && !stderr.Contains("no such file", StringComparison.OrdinalIgnoreCase);
    }
}

/// <summary>
/// Statistics about resilience patterns for monitoring.
/// </summary>
public record ResilienceStatistics
{
    /// <summary>
    /// Total number of retries performed.
    /// </summary>
    public int TotalRetries { get; init; }

    /// <summary>
    /// Number of times circuit breaker opened.
    /// </summary>
    public int CircuitBreakerOpenings { get; init; }

    /// <summary>
    /// Current circuit breaker state.
    /// </summary>
    public string CircuitBreakerState { get; init; } = "Unknown";
}
