using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "automation-rule-set", "create")]
public record AzSecurityAutomationRuleSetCreateOptions : AzOptions
{
    [CommandSwitch("--rules")]
    public string? Rules { get; set; }
}