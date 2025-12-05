using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotanalytics", "sample-channel-data")]
public record AwsIotanalyticsSampleChannelDataOptions(
[property: CliOption("--channel-name")] string ChannelName
) : AwsOptions
{
    [CliOption("--max-messages")]
    public int? MaxMessages { get; set; }

    [CliOption("--start-time")]
    public long? StartTime { get; set; }

    [CliOption("--end-time")]
    public long? EndTime { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}