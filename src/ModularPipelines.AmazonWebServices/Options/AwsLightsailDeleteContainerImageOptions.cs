using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "delete-container-image")]
public record AwsLightsailDeleteContainerImageOptions(
[property: CommandSwitch("--service-name")] string ServiceName,
[property: CommandSwitch("--image")] string Image
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}