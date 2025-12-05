using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("security", "assessment", "show")]
public record AzSecurityAssessmentShowOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--assessed-resource-id")]
    public string? AssessedResourceId { get; set; }
}