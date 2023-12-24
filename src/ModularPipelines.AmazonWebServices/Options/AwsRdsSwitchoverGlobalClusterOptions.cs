using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "switchover-global-cluster")]
public record AwsRdsSwitchoverGlobalClusterOptions(
[property: CommandSwitch("--global-cluster-identifier")] string GlobalClusterIdentifier,
[property: CommandSwitch("--target-db-cluster-identifier")] string TargetDbClusterIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}