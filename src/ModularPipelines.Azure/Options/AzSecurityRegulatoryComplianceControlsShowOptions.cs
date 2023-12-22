using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "regulatory-compliance-controls", "show")]
public record AzSecurityRegulatoryComplianceControlsShowOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--standard-name")] string StandardName
) : AzOptions;