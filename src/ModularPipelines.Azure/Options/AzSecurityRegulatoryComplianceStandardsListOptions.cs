using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "regulatory-compliance-standards", "list")]
public record AzSecurityRegulatoryComplianceStandardsListOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
}

