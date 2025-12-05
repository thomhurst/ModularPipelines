using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transcribe", "start-call-analytics-job")]
public record AwsTranscribeStartCallAnalyticsJobOptions(
[property: CliOption("--call-analytics-job-name")] string CallAnalyticsJobName,
[property: CliOption("--media")] string Media
) : AwsOptions
{
    [CliOption("--output-location")]
    public string? OutputLocation { get; set; }

    [CliOption("--output-encryption-kms-key-id")]
    public string? OutputEncryptionKmsKeyId { get; set; }

    [CliOption("--data-access-role-arn")]
    public string? DataAccessRoleArn { get; set; }

    [CliOption("--settings")]
    public string? Settings { get; set; }

    [CliOption("--channel-definitions")]
    public string[]? ChannelDefinitions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}