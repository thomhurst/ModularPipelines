using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediapackagev2", "delete-origin-endpoint-policy")]
public record AwsMediapackagev2DeleteOriginEndpointPolicyOptions(
[property: CommandSwitch("--channel-group-name")] string ChannelGroupName,
[property: CommandSwitch("--channel-name")] string ChannelName,
[property: CommandSwitch("--origin-endpoint-name")] string OriginEndpointName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}