using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "automation-rule-set", "create")]
public record AzSecurityAutomationRuleSetCreateOptions : AzOptions
{
    [CommandSwitch("--rules")]
    public string? Rules { get; set; }
}