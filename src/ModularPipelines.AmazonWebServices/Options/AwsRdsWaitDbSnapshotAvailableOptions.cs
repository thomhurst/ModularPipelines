using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "wait", "db-snapshot-available")]
public record AwsRdsWaitDbSnapshotAvailableOptions : AwsOptions
{
    [CliOption("--db-instance-identifier")]
    public string? DbInstanceIdentifier { get; set; }

    [CliOption("--db-snapshot-identifier")]
    public string? DbSnapshotIdentifier { get; set; }

    [CliOption("--snapshot-type")]
    public string? SnapshotType { get; set; }

    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--dbi-resource-id")]
    public string? DbiResourceId { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}