using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("docdb", "create-db-cluster-snapshot")]
public record AwsDocdbCreateDbClusterSnapshotOptions(
[property: CommandSwitch("--db-cluster-snapshot-identifier")] string DbClusterSnapshotIdentifier,
[property: CommandSwitch("--db-cluster-identifier")] string DbClusterIdentifier
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}