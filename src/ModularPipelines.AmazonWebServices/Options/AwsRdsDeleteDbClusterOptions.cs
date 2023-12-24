using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "delete-db-cluster")]
public record AwsRdsDeleteDbClusterOptions(
[property: CommandSwitch("--db-cluster-identifier")] string DbClusterIdentifier
) : AwsOptions
{
    [CommandSwitch("--final-db-snapshot-identifier")]
    public string? FinalDbSnapshotIdentifier { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}