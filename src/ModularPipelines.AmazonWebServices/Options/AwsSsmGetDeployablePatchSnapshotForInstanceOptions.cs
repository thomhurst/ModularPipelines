using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "get-deployable-patch-snapshot-for-instance")]
public record AwsSsmGetDeployablePatchSnapshotForInstanceOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--snapshot-id")] string SnapshotId
) : AwsOptions
{
    [CliOption("--baseline-override")]
    public string? BaselineOverride { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}