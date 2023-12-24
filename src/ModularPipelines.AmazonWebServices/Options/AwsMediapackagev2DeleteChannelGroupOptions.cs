using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediapackagev2", "delete-channel-group")]
public record AwsMediapackagev2DeleteChannelGroupOptions(
[property: CommandSwitch("--channel-group-name")] string ChannelGroupName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}