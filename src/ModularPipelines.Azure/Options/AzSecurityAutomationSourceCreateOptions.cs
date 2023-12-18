using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "automation-source", "create")]
public record AzSecurityAutomationSourceCreateOptions(
[property: CommandSwitch("--event-source")] string EventSource
) : AzOptions
{
    [CommandSwitch("--rule-sets")]
    public string? RuleSets { get; set; }
}