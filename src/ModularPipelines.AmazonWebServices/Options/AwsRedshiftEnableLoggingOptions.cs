using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "enable-logging")]
public record AwsRedshiftEnableLoggingOptions(
[property: CommandSwitch("--cluster-identifier")] string ClusterIdentifier
) : AwsOptions
{
    [CommandSwitch("--bucket-name")]
    public string? BucketName { get; set; }

    [CommandSwitch("--s3-key-prefix")]
    public string? S3KeyPrefix { get; set; }

    [CommandSwitch("--log-destination-type")]
    public string? LogDestinationType { get; set; }

    [CommandSwitch("--log-exports")]
    public string[]? LogExports { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}