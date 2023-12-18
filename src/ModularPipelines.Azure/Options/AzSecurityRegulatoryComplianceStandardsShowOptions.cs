using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "regulatory-compliance-standards", "show")]
public record AzSecurityRegulatoryComplianceStandardsShowOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
}

