using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "failover-global-cluster")]
public record AwsRdsFailoverGlobalClusterOptions(
[property: CommandSwitch("--global-cluster-identifier")] string GlobalClusterIdentifier,
[property: CommandSwitch("--target-db-cluster-identifier")] string TargetDbClusterIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}