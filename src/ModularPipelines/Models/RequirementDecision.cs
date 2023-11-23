using System.Text.Json.Serialization;

namespace ModularPipelines.Models;

public sealed record RequirementDecision
{
    [JsonInclude]
    public bool Success { get; private set; }

    [JsonInclude]
    public string? Reason { get; private init; }

    [JsonConstructor]
    private RequirementDecision()
    {
    }

    private RequirementDecision(bool success)
    {
        Success = success;
    }

    public static readonly RequirementDecision Passed = new(true);

    public static RequirementDecision Failed(string? reason) => new(false)
    {
        Reason = reason,
    };
    
    public static RequirementDecision Of(bool passed, string? reason) => new(passed)
    {
        Reason = passed ? null : reason,
    };

    public static implicit operator RequirementDecision(bool shouldSkip) => shouldSkip ? Failed(null) : Passed;

    public static implicit operator RequirementDecision(string reason) => Failed(reason);
}