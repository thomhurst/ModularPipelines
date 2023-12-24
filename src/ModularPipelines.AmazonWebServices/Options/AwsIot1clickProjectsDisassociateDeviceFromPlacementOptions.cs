using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot1click-projects", "disassociate-device-from-placement")]
public record AwsIot1clickProjectsDisassociateDeviceFromPlacementOptions(
[property: CommandSwitch("--project-name")] string ProjectName,
[property: CommandSwitch("--placement-name")] string PlacementName,
[property: CommandSwitch("--device-template-name")] string DeviceTemplateName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}