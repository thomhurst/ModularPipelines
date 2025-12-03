using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "modify-db-snapshot-attribute")]
public record AwsRdsModifyDbSnapshotAttributeOptions(
[property: CliOption("--db-snapshot-identifier")] string DbSnapshotIdentifier,
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