using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "regulatory-compliance-controls", "list")]
public record AzSecurityRegulatoryComplianceControlsListOptions(
[property: CommandSwitch("--standard-name")] string StandardName
) : AzOptions
{
}