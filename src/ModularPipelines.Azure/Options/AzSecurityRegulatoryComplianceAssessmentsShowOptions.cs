using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "regulatory-compliance-assessments", "show")]
public record AzSecurityRegulatoryComplianceAssessmentsShowOptions(
[property: CommandSwitch("--control-name")] string ControlName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--standard-name")] string StandardName
) : AzOptions;