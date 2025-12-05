using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "failover-global-cluster")]
public record AwsRdsFailoverGlobalClusterOptions(
[property: CliOption("--global-cluster-identifier")] string GlobalClusterIdentifier,
[property: CliOption("--target-db-cluster-identifier")] string TargetDbClusterIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}