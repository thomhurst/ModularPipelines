using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "regulatory-compliance-standards", "show")]
public record AzSecurityRegulatoryComplianceStandardsShowOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;