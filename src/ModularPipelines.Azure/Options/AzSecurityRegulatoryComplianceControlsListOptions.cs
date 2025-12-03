using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("security", "regulatory-compliance-controls", "list")]
public record AzSecurityRegulatoryComplianceControlsListOptions(
[property: CliOption("--standard-name")] string StandardName
) : AzOptions;