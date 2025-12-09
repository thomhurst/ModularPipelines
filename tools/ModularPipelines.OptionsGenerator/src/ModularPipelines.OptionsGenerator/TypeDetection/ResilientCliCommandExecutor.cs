using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace ModularPipelines.OptionsGenerator.TypeDetection;

/// <summary>
/// Decorator that adds resilience patterns (retry and circuit breaker) to CLI command execution.
/// Uses Polly for:
/// - Exponential backoff retry (3 attempts) for transient failures
/// - Circuit breaker (5 failures opens circuit for 30 seconds) to prevent cascading failures
/// </summary>
public sealed class ResilientCliCommandExecutor : ICliCommandExecutor
{
    private readonly ICliCommandExecutor _inner;
    private readonly ILogger<ResilientCliCommandExecutor> _logger;
    private readonly ResiliencePipeline<CliCommandResult> _pipeline;

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

        _pipeline = new ResiliencePipelineBuilder<CliCommandResult>()
            .AddRetry(new RetryStrategyOptions<CliCommandResult>
            {
                MaxRetryAttempts = maxRetries,
                BackoffType = DelayBackoffType.Exponential,
                Delay = baseDelay,
                ShouldHandle = new PredicateBuilder<CliCommandResult>()
                    .HandleResult(r => IsTransientFailure(r)),
                OnRetry = args =>
                {
                    _logger.LogWarning(
                        "CLI command failed (attempt {Attempt}/{MaxAttempts}), retrying in {Delay}ms...",
                        args.AttemptNumber,
                        maxRetries,
                        args.RetryDelay.TotalMilliseconds);
                    return default;
                }
            })
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions<CliCommandResult>
            {
                FailureRatio = 0.5,
                MinimumThroughput = circuitBreakerThreshold,
                SamplingDuration = TimeSpan.FromSeconds(30),
                BreakDuration = circuitBreakerDuration,
                ShouldHandle = new PredicateBuilder<CliCommandResult>()
                    .HandleResult(r => IsTransientFailure(r)),
                OnOpened = args =>
                {
                    _logger.LogError(
                        "Circuit breaker OPENED - CLI commands are failing. Will retry after {Duration}s",
                        args.BreakDuration.TotalSeconds);
                    return default;
                },
                OnClosed = _ =>
                {
                    _logger.LogInformation("Circuit breaker CLOSED - CLI commands are healthy again");
                    return default;
                },
                OnHalfOpened = _ =>
                {
                    _logger.LogInformation("Circuit breaker HALF-OPEN - Testing if CLI commands are healthy...");
                    return default;
                }
            })
            .Build();
    }

    public async Task<CliCommandResult> ExecuteAsync(
        string command,
        string arguments,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _pipeline.ExecuteAsync(
                async token => await _inner.ExecuteAsync(command, arguments, token),
                cancellationToken);
        }
        catch (BrokenCircuitException ex)
        {
            _logger.LogError("Circuit breaker is open - CLI execution rejected: {Command} {Arguments}", command, arguments);
            return new CliCommandResult
            {
                StandardOutput = string.Empty,
                StandardError = $"Circuit breaker open: {ex.Message}",
                ExitCode = -2 // Special exit code for circuit breaker rejection
            };
        }
    }

    public async Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default)
    {
        // Availability check doesn't use resilience patterns - we want immediate feedback
        return await _inner.IsAvailableAsync(command, cancellationToken);
    }

    /// <summary>
    /// Determines if a failure is transient and should trigger retry.
    /// Transient failures include:
    /// - Timeout (exit code -1)
    /// - Process errors that aren't "command not found"
    /// </summary>
    private static bool IsTransientFailure(CliCommandResult result)
    {
        // Exit code -1 typically means timeout or execution failure
        if (result.ExitCode == -1)
        {
            // Check if it's "command not found" vs transient failure
            var stderr = result.StandardError?.ToLowerInvariant() ?? "";

            // Don't retry if command doesn't exist
            if (stderr.Contains("not found") ||
                stderr.Contains("not recognized") ||
                stderr.Contains("cannot find") ||
                stderr.Contains("no such file"))
            {
                return false;
            }

            // Retry on timeout or other transient errors
            return true;
        }

        // Non-zero exit codes from CLI tools are usually intentional (validation, errors, etc.)
        // We only retry on system-level failures, not CLI-level errors
        return false;
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
