using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("security", "regulatory-compliance-controls", "show")]
public record AzSecurityRegulatoryComplianceControlsShowOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--standard-name")] string StandardName
) : AzOptions;