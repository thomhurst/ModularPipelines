using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "regulatory-compliance-assessments", "list")]
public record AzSecurityRegulatoryComplianceAssessmentsListOptions(
[property: CommandSwitch("--control-name")] string ControlName,
[property: CommandSwitch("--standard-name")] string StandardName
) : AzOptions;