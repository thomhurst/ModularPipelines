using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("security", "assessment", "delete")]
public record AzSecurityAssessmentDeleteOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--assessed-resource-id")]
    public string? AssessedResourceId { get; set; }
}