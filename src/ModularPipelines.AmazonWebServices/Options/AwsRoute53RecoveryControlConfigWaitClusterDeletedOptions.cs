using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53-recovery-control-config", "wait", "cluster-deleted")]
public record AwsRoute53RecoveryControlConfigWaitClusterDeletedOptions(
[property: CliOption("--cluster-arn")] string ClusterArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}