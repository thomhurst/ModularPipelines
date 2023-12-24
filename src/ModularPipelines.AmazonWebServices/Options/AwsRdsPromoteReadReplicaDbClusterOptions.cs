using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "promote-read-replica-db-cluster")]
public record AwsRdsPromoteReadReplicaDbClusterOptions(
[property: CommandSwitch("--db-cluster-identifier")] string DbClusterIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}