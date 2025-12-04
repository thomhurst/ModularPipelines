using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("security", "automation-source", "create")]
public record AzSecurityAutomationSourceCreateOptions(
[property: CliOption("--event-source")] string EventSource
) : AzOptions
{
    [CliOption("--rule-sets")]
    public string? RuleSets { get; set; }
}