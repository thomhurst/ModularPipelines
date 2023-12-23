using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesisanalyticsv2", "create-application-presigned-url")]
public record AwsKinesisanalyticsv2CreateApplicationPresignedUrlOptions(
[property: CommandSwitch("--application-name")] string ApplicationName,
[property: CommandSwitch("--url-type")] string UrlType
) : AwsOptions
{
    [CommandSwitch("--session-expiration-duration-in-seconds")]
    public long? SessionExpirationDurationInSeconds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}