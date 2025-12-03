using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesisanalyticsv2", "create-application-presigned-url")]
public record AwsKinesisanalyticsv2CreateApplicationPresignedUrlOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--url-type")] string UrlType
) : AwsOptions
{
    [CliOption("--session-expiration-duration-in-seconds")]
    public long? SessionExpirationDurationInSeconds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}