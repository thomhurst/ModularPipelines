using System.Text.Json.Serialization;
using ModularPipelines.Engine;
using ModularPipelines.Enums;

namespace ModularPipelines.Models;

/// <summary>
/// Represents the result of a module execution as a discriminated union.
/// Use pattern matching to handle Success, Failure, and Skipped cases.
/// </summary>
/// <typeparam name="T">The type of value returned on success.</typeparam>
/// <example>
/// <code>
/// var result = await myModule;
/// switch (result)
/// {
///     case ModuleResult&lt;string&gt;.Success { Value: var value }:
///         Console.WriteLine($"Got: {value}");
///         break;
///     case ModuleResult&lt;string&gt;.Failure { Exception: var ex }:
///         Console.WriteLine($"Failed: {ex.Message}");
///         break;
///     case ModuleResult&lt;string&gt;.Skipped { Decision: var skip }:
///         Console.WriteLine($"Skipped: {skip.Reason}");
///         break;
/// }
/// </code>
/// </example>
public abstract record ModuleResult<T> : IModuleResult
{
    // === Metadata (available on all outcomes) ===

    /// <inheritdoc />
    [JsonInclude]
    public required string ModuleName { get; init; }

    /// <inheritdoc />
    [JsonInclude]
    public required TimeSpan ModuleDuration { get; init; }

    /// <inheritdoc />
    [JsonInclude]
    public required DateTimeOffset ModuleStart { get; init; }

    /// <inheritdoc />
    [JsonInclude]
    public required DateTimeOffset ModuleEnd { get; init; }

    /// <inheritdoc />
    [JsonInclude]
    public required Status ModuleStatus { get; init; }

    // === Quick checks ===

    /// <inheritdoc />
    [JsonIgnore]
    public bool IsSuccess => this is Success;

    /// <inheritdoc />
    [JsonIgnore]
    public bool IsFailure => this is Failure;

    /// <inheritdoc />
    [JsonIgnore]
    public bool IsSkipped => this is Skipped;

    // === Safe accessors (no exceptions) ===

    /// <summary>
    /// Gets the value if successful, or default(T) otherwise. Does not throw.
    /// </summary>
    [JsonIgnore]
    public T? ValueOrDefault => this is Success s ? s.Value : default;

    /// <inheritdoc />
    [JsonIgnore]
    object? IModuleResult.ValueOrDefault => ValueOrDefault;

    /// <inheritdoc />
    [JsonIgnore]
    public Exception? ExceptionOrDefault => this is Failure f ? f.Exception : null;

    /// <inheritdoc />
    [JsonIgnore]
    public SkipDecision? SkipDecisionOrDefault => this is Skipped sk ? sk.Decision : null;

    // === Computed for compatibility ===

    /// <inheritdoc />
    [JsonIgnore]
    public ModuleResultType ModuleResultType => this switch
    {
        Success => ModuleResultType.Success,
        Failure => ModuleResultType.Failure,
        Skipped => ModuleResultType.Skipped,
        _ => throw new InvalidOperationException("Unknown result type")
    };

    // === Internal: Module type tracking ===

    /// <summary>
    /// Gets or sets the type of the module that produced this result.
    /// </summary>
    [JsonIgnore]
    internal Type? ModuleType { get; init; }

    // === Pattern matching helpers ===

    /// <summary>
    /// Matches the result to one of three functions based on the outcome.
    /// </summary>
    /// <typeparam name="TResult">The return type of the match functions.</typeparam>
    /// <param name="onSuccess">Function to call if successful.</param>
    /// <param name="onFailure">Function to call if failed.</param>
    /// <param name="onSkipped">Function to call if skipped.</param>
    /// <returns>The result of the matched function.</returns>
    public TResult Match<TResult>(
        Func<T, TResult> onSuccess,
        Func<Exception, TResult> onFailure,
        Func<SkipDecision, TResult> onSkipped) => this switch
    {
        Success s => onSuccess(s.Value),
        Failure f => onFailure(f.Exception),
        Skipped sk => onSkipped(sk.Decision),
        _ => throw new InvalidOperationException("Unknown result type")
    };

    /// <summary>
    /// Executes one of three actions based on the outcome.
    /// </summary>
    /// <param name="onSuccess">Action to call if successful.</param>
    /// <param name="onFailure">Action to call if failed.</param>
    /// <param name="onSkipped">Action to call if skipped.</param>
    public void Switch(
        Action<T> onSuccess,
        Action<Exception> onFailure,
        Action<SkipDecision> onSkipped)
    {
        switch (this)
        {
            case Success s:
                onSuccess(s.Value);
                break;
            case Failure f:
                onFailure(f.Exception);
                break;
            case Skipped sk:
                onSkipped(sk.Decision);
                break;
        }
    }

    // === Discriminated variants ===

    /// <summary>
    /// Represents a successful module execution with a value.
    /// </summary>
    /// <param name="Value">The value produced by the module.</param>
    public sealed record Success(T Value) : ModuleResult<T>;

    /// <summary>
    /// Represents a failed module execution with an exception.
    /// </summary>
    /// <param name="Exception">The exception that caused the failure.</param>
    public sealed record Failure(Exception Exception) : ModuleResult<T>;

    /// <summary>
    /// Represents a skipped module execution.
    /// </summary>
    /// <param name="Decision">The skip decision containing the reason.</param>
    public sealed record Skipped(SkipDecision Decision) : ModuleResult<T>;

    // === Internal factory methods ===

    internal static Success CreateSuccess(T value, ModuleExecutionContext ctx) => new(value)
    {
        ModuleName = ctx.ModuleType.Name,
        ModuleDuration = (ctx.EndTime == DateTimeOffset.MinValue ? DateTimeOffset.Now : ctx.EndTime) -
                        (ctx.StartTime == DateTimeOffset.MinValue ? DateTimeOffset.Now : ctx.StartTime),
        ModuleStart = ctx.StartTime == DateTimeOffset.MinValue ? DateTimeOffset.Now : ctx.StartTime,
        ModuleEnd = ctx.EndTime == DateTimeOffset.MinValue ? DateTimeOffset.Now : ctx.EndTime,
        ModuleStatus = ctx.Status,
        ModuleType = ctx.ModuleType
    };

    internal static Failure CreateFailure(Exception exception, ModuleExecutionContext ctx) => new(exception)
    {
        ModuleName = ctx.ModuleType.Name,
        ModuleDuration = (ctx.EndTime == DateTimeOffset.MinValue ? DateTimeOffset.Now : ctx.EndTime) -
                        (ctx.StartTime == DateTimeOffset.MinValue ? DateTimeOffset.Now : ctx.StartTime),
        ModuleStart = ctx.StartTime == DateTimeOffset.MinValue ? DateTimeOffset.Now : ctx.StartTime,
        ModuleEnd = ctx.EndTime == DateTimeOffset.MinValue ? DateTimeOffset.Now : ctx.EndTime,
        ModuleStatus = ctx.Status,
        ModuleType = ctx.ModuleType
    };

    internal static Skipped CreateSkipped(SkipDecision decision, ModuleExecutionContext ctx) => new(decision)
    {
        ModuleName = ctx.ModuleType.Name,
        ModuleDuration = (ctx.EndTime == DateTimeOffset.MinValue ? DateTimeOffset.Now : ctx.EndTime) -
                        (ctx.StartTime == DateTimeOffset.MinValue ? DateTimeOffset.Now : ctx.StartTime),
        ModuleStart = ctx.StartTime == DateTimeOffset.MinValue ? DateTimeOffset.Now : ctx.StartTime,
        ModuleEnd = ctx.EndTime == DateTimeOffset.MinValue ? DateTimeOffset.Now : ctx.EndTime,
        ModuleStatus = ctx.Status,
        ModuleType = ctx.ModuleType
    };

    // Prevent external inheritance - only Success, Failure, Skipped are valid
    private protected ModuleResult()
    {
    }
}
