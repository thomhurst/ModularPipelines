using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("security", "regulatory-compliance-standards", "show")]
public record AzSecurityRegulatoryComplianceStandardsShowOptions(
[property: CliOption("--name")] string Name
) : AzOptions;