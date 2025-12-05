using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("security", "automation-rule-set", "create")]
public record AzSecurityAutomationRuleSetCreateOptions : AzOptions
{
    [CliOption("--rules")]
    public string? Rules { get; set; }
}