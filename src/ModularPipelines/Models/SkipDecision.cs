using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ModularPipelines.Models;

public sealed record SkipDecision
{
    [JsonInclude]
    public bool ShouldSkip { get; private set; }

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

    public static readonly SkipDecision DoNotSkip = new(false);

    public static SkipDecision Skip(string? reason) => new(true)
    {
        Reason = reason,
    };

    public static SkipDecision Of(bool shouldSkip, string? reason) => new(shouldSkip)
    {
        Reason = shouldSkip ? reason : null,
    };

    public static implicit operator SkipDecision(bool shouldSkip) => shouldSkip ? Skip(null) : DoNotSkip;

    public static implicit operator SkipDecision(string reason) => Skip(reason);

    public static implicit operator Task<SkipDecision>(SkipDecision skipDecision) => Task.FromResult(skipDecision);
}