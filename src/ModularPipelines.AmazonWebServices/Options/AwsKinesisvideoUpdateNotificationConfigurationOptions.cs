using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesisvideo", "update-notification-configuration")]
public record AwsKinesisvideoUpdateNotificationConfigurationOptions : AwsOptions
{
    [CliOption("--stream-name")]
    public string? StreamName { get; set; }

    [CliOption("--stream-arn")]
    public string? StreamArn { get; set; }

    [CliOption("--notification-configuration")]
    public string? NotificationConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}