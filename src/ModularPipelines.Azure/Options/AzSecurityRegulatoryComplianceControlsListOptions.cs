using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "regulatory-compliance-controls", "list")]
public record AzSecurityRegulatoryComplianceControlsListOptions(
[property: CommandSwitch("--standard-name")] string StandardName
) : AzOptions;