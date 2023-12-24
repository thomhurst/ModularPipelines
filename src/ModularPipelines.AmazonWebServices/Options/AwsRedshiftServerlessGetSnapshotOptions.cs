using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift-serverless", "get-snapshot")]
public record AwsRedshiftServerlessGetSnapshotOptions : AwsOptions
{
    [CommandSwitch("--owner-account")]
    public string? OwnerAccount { get; set; }

    [CommandSwitch("--snapshot-arn")]
    public string? SnapshotArn { get; set; }

    [CommandSwitch("--snapshot-name")]
    public string? SnapshotName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}