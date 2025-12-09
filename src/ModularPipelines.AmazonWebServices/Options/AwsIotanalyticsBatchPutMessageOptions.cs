using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotanalytics", "batch-put-message")]
public record AwsIotanalyticsBatchPutMessageOptions(
[property: CliOption("--channel-name")] string ChannelName,
[property: CliOption("--messages")] string[] Messages
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}