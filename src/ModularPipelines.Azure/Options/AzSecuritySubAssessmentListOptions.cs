using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("security", "sub-assessment", "list")]
public record AzSecuritySubAssessmentListOptions : AzOptions
{
    [CliOption("--assessed-resource-id")]
    public string? AssessedResourceId { get; set; }

    [CliOption("--assessment-name")]
    public string? AssessmentName { get; set; }
}