using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("security", "regulatory-compliance-assessments", "list")]
public record AzSecurityRegulatoryComplianceAssessmentsListOptions(
[property: CliOption("--control-name")] string ControlName,
[property: CliOption("--standard-name")] string StandardName
) : AzOptions;