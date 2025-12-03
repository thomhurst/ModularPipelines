using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot1click-projects", "get-devices-in-placement")]
public record AwsIot1clickProjectsGetDevicesInPlacementOptions(
[property: CliOption("--project-name")] string ProjectName,
[property: CliOption("--placement-name")] string PlacementName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}