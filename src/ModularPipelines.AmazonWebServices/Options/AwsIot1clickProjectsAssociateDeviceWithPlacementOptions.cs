using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot1click-projects", "associate-device-with-placement")]
public record AwsIot1clickProjectsAssociateDeviceWithPlacementOptions(
[property: CliOption("--project-name")] string ProjectName,
[property: CliOption("--placement-name")] string PlacementName,
[property: CliOption("--device-id")] string DeviceId,
[property: CliOption("--device-template-name")] string DeviceTemplateName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}