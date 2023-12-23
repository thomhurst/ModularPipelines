using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "enable-add-on")]
public record AwsLightsailEnableAddOnOptions(
[property: CommandSwitch("--resource-name")] string ResourceName,
[property: CommandSwitch("--add-on-request")] string AddOnRequest
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}