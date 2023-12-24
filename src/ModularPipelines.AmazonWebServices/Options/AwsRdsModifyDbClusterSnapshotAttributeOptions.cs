using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "modify-db-cluster-snapshot-attribute")]
public record AwsRdsModifyDbClusterSnapshotAttributeOptions(
[property: CommandSwitch("--db-cluster-snapshot-identifier")] string DbClusterSnapshotIdentifier,
[property: CommandSwitch("--attribute-name")] string AttributeName
) : AwsOptions
{
    [CommandSwitch("--values-to-add")]
    public string[]? ValuesToAdd { get; set; }

    [CommandSwitch("--values-to-remove")]
    public string[]? ValuesToRemove { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}