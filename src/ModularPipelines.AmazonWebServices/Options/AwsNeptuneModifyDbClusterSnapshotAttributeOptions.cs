using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptune", "modify-db-cluster-snapshot-attribute")]
public record AwsNeptuneModifyDbClusterSnapshotAttributeOptions(
[property: CliOption("--db-cluster-snapshot-identifier")] string DbClusterSnapshotIdentifier,
[property: CliOption("--attribute-name")] string AttributeName
) : AwsOptions
{
    [CliOption("--values-to-add")]
    public string[]? ValuesToAdd { get; set; }

    [CliOption("--values-to-remove")]
    public string[]? ValuesToRemove { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}