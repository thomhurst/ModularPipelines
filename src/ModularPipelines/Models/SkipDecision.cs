using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ModularPipelines.Models;

/// <summary>
/// Represents the result of evaluating whether a module should be skipped.
/// </summary>
public sealed record SkipDecision
{
    /// <summary>
    /// Gets a value indicating whether the module should be skipped.
    /// </summary>
    [JsonInclude]
    public bool ShouldSkip { get; private init; }

    /// <summary>
    /// Gets the reason the module should be skipped, or <see langword="null"/> when no reason was provided.
    /// </summary>
    [JsonInclude]
    public string? Reason { get; private init; }

    [ExcludeFromCodeCoverage]
    [JsonConstructor]
    private SkipDecision()
    {
    }

    private SkipDecision(bool shouldSkip)
    {
        ShouldSkip = shouldSkip;
    }

    /// <summary>
    /// Gets a decision that allows the module to run.
    /// </summary>
    public static readonly SkipDecision DoNotSkip = new(false);

    /// <summary>
    /// Creates a decision that skips the module.
    /// </summary>
    /// <param name="reason">The reason for skipping the module, or <see langword="null"/> when unspecified.</param>
    /// <returns>A decision that skips the module.</returns>
    public static SkipDecision Skip(string? reason) => new(true)
    {
        Reason = reason,
    };

    /// <summary>
    /// Creates a skip decision from a boolean condition.
    /// </summary>
    /// <param name="shouldSkip"><see langword="true"/> to skip the module; otherwise, <see langword="false"/>.</param>
    /// <param name="reason">The reason for skipping, used only when <paramref name="shouldSkip"/> is <see langword="true"/>.</param>
    /// <returns>A decision matching <paramref name="shouldSkip"/>.</returns>
    public static SkipDecision When(bool shouldSkip, string? reason) => new(shouldSkip)
    {
        Reason = shouldSkip ? reason : null,
    };

    /// <summary>
    /// Creates a skip decision from a boolean condition.
    /// </summary>
    /// <param name="shouldSkip"><see langword="true"/> to skip the module; otherwise, <see langword="false"/>.</param>
    /// <param name="reason">The reason for skipping, used only when <paramref name="shouldSkip"/> is <see langword="true"/>.</param>
    /// <returns>A decision matching <paramref name="shouldSkip"/>.</returns>
    [Obsolete("Use When(bool, string?) instead.")]
    public static SkipDecision Of(bool shouldSkip, string? reason) => When(shouldSkip, reason);
}
