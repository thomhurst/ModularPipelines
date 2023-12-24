using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediapackagev2", "put-channel-policy")]
public record AwsMediapackagev2PutChannelPolicyOptions(
[property: CommandSwitch("--channel-group-name")] string ChannelGroupName,
[property: CommandSwitch("--channel-name")] string ChannelName,
[property: CommandSwitch("--policy")] string Policy
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}