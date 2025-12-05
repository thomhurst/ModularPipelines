using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot1click-projects", "delete-placement")]
public record AwsIot1clickProjectsDeletePlacementOptions(
[property: CliOption("--placement-name")] string PlacementName,
[property: CliOption("--project-name")] string ProjectName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}