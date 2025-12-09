using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift-serverless", "get-snapshot")]
public record AwsRedshiftServerlessGetSnapshotOptions : AwsOptions
{
    [CliOption("--owner-account")]
    public string? OwnerAccount { get; set; }

    [CliOption("--snapshot-arn")]
    public string? SnapshotArn { get; set; }

    [CliOption("--snapshot-name")]
    public string? SnapshotName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}