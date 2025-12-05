using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53-recovery-control-config", "delete-cluster")]
public record AwsRoute53RecoveryControlConfigDeleteClusterOptions(
[property: CliOption("--cluster-arn")] string ClusterArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}