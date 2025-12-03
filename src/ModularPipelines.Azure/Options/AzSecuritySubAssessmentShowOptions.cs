using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("security", "sub-assessment", "show")]
public record AzSecuritySubAssessmentShowOptions(
[property: CliOption("--assessment-name")] string AssessmentName,
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--assessed-resource-id")]
    public string? AssessedResourceId { get; set; }
}