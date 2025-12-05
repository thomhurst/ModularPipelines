using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot1click-projects", "disassociate-device-from-placement")]
public record AwsIot1clickProjectsDisassociateDeviceFromPlacementOptions(
[property: CliOption("--project-name")] string ProjectName,
[property: CliOption("--placement-name")] string PlacementName,
[property: CliOption("--device-template-name")] string DeviceTemplateName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}