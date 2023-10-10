using System.Text.Json.Serialization;

namespace ModularPipelines.Models;

public sealed record SkipDecision
{
    [JsonInclude]
    public bool ShouldSkip { get; private set; }

    [JsonInclude]
    public string? Reason { get; private init; }

    [JsonConstructor]
    private SkipDecision()
    {
    }

    private SkipDecision(bool shouldSkip)
    {
        ShouldSkip = shouldSkip;
    }

    public static readonly SkipDecision DoNotSkip = new SkipDecision(false);

    public static SkipDecision Skip(string? reason) => new SkipDecision(true)
    {
        Reason = reason,
    };

    public static implicit operator SkipDecision(bool shouldSkip) => shouldSkip ? Skip(null) : DoNotSkip;

    public static implicit operator SkipDecision(string reason) => Skip(reason);
}