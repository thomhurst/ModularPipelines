using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("security", "regulatory-compliance-assessments", "show")]
public record AzSecurityRegulatoryComplianceAssessmentsShowOptions(
[property: CliOption("--control-name")] string ControlName,
[property: CliOption("--name")] string Name,
[property: CliOption("--standard-name")] string StandardName
) : AzOptions;