using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "enable-logging")]
public record AwsRedshiftEnableLoggingOptions(
[property: CliOption("--cluster-identifier")] string ClusterIdentifier
) : AwsOptions
{
    [CliOption("--bucket-name")]
    public string? BucketName { get; set; }

    [CliOption("--s3-key-prefix")]
    public string? S3KeyPrefix { get; set; }

    [CliOption("--log-destination-type")]
    public string? LogDestinationType { get; set; }

    [CliOption("--log-exports")]
    public string[]? LogExports { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}