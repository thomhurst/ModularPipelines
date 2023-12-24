using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediapackagev2", "delete-channel")]
public record AwsMediapackagev2DeleteChannelOptions(
[property: CommandSwitch("--channel-group-name")] string ChannelGroupName,
[property: CommandSwitch("--channel-name")] string ChannelName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}