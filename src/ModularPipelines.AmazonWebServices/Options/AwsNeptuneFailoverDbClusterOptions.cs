using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("neptune", "failover-db-cluster")]
public record AwsNeptuneFailoverDbClusterOptions : AwsOptions
{
    [CommandSwitch("--db-cluster-identifier")]
    public string? DbClusterIdentifier { get; set; }

    [CommandSwitch("--target-db-instance-identifier")]
    public string? TargetDbInstanceIdentifier { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}