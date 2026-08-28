using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ModularPipelines.Models;

public sealed record RequirementDecision
{
    [JsonInclude]
    public bool IsSatisfied { get; private set; }

    [JsonInclude]
    public string? Reason { get; private init; }

    [ExcludeFromCodeCoverage]
    [JsonConstructor]
    private RequirementDecision()
    {
    }

    private RequirementDecision(bool isSatisfied)
    {
        IsSatisfied = isSatisfied;
    }

    public static readonly RequirementDecision Passed = new(true);

    public static RequirementDecision Failed(string? reason) => new(false)
    {
        Reason = reason,
    };

    public static implicit operator RequirementDecision(bool passed) => passed ? Passed : Failed(null);
}
