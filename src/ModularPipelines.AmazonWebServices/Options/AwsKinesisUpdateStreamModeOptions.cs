using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesis", "update-stream-mode")]
public record AwsKinesisUpdateStreamModeOptions(
[property: CommandSwitch("--stream-arn")] string StreamArn,
[property: CommandSwitch("--stream-mode-details")] string StreamModeDetails
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}