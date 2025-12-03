using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("security", "assessment-metadata", "create")]
public record AzSecurityAssessmentMetadataCreateOptions(
[property: CliOption("--description")] string Description,
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--name")] string Name,
[property: CliOption("--severity")] string Severity
) : AzOptions
{
    [CliOption("--remediation-description")]
    public string? RemediationDescription { get; set; }
}