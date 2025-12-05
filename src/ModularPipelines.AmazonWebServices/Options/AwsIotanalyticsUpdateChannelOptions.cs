using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotanalytics", "update-channel")]
public record AwsIotanalyticsUpdateChannelOptions(
[property: CliOption("--channel-name")] string ChannelName
) : AwsOptions
{
    [CliOption("--channel-storage")]
    public string? ChannelStorage { get; set; }

    [CliOption("--retention-period")]
    public string? RetentionPeriod { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}