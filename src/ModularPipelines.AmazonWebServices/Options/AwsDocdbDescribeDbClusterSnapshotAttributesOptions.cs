using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("docdb", "describe-db-cluster-snapshot-attributes")]
public record AwsDocdbDescribeDbClusterSnapshotAttributesOptions(
[property: CommandSwitch("--db-cluster-snapshot-identifier")] string DbClusterSnapshotIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}