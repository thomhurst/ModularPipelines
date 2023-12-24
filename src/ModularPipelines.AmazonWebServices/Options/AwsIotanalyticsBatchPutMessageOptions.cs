using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotanalytics", "batch-put-message")]
public record AwsIotanalyticsBatchPutMessageOptions(
[property: CommandSwitch("--channel-name")] string ChannelName,
[property: CommandSwitch("--messages")] string[] Messages
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}