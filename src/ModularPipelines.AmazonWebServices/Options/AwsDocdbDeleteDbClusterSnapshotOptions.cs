using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("docdb", "delete-db-cluster-snapshot")]
public record AwsDocdbDeleteDbClusterSnapshotOptions(
[property: CommandSwitch("--db-cluster-snapshot-identifier")] string DbClusterSnapshotIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}