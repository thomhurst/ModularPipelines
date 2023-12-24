using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sqs", "start-message-move-task")]
public record AwsSqsStartMessageMoveTaskOptions(
[property: CommandSwitch("--source-arn")] string SourceArn
) : AwsOptions
{
    [CommandSwitch("--destination-arn")]
    public string? DestinationArn { get; set; }

    [CommandSwitch("--max-number-of-messages-per-second")]
    public int? MaxNumberOfMessagesPerSecond { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}