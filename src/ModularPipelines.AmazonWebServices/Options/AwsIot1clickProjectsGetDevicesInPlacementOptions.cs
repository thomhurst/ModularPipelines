using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot1click-projects", "get-devices-in-placement")]
public record AwsIot1clickProjectsGetDevicesInPlacementOptions(
[property: CommandSwitch("--project-name")] string ProjectName,
[property: CommandSwitch("--placement-name")] string PlacementName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}