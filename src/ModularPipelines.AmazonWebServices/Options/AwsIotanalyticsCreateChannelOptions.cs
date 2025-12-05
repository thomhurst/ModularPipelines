using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotanalytics", "create-channel")]
public record AwsIotanalyticsCreateChannelOptions(
[property: CliOption("--channel-name")] string ChannelName
) : AwsOptions
{
    [CliOption("--channel-storage")]
    public string? ChannelStorage { get; set; }

    [CliOption("--retention-period")]
    public string? RetentionPeriod { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}