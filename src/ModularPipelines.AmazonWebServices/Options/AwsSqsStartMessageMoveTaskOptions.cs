using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sqs", "start-message-move-task")]
public record AwsSqsStartMessageMoveTaskOptions(
[property: CliOption("--source-arn")] string SourceArn
) : AwsOptions
{
    [CliOption("--destination-arn")]
    public string? DestinationArn { get; set; }

    [CliOption("--max-number-of-messages-per-second")]
    public int? MaxNumberOfMessagesPerSecond { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}