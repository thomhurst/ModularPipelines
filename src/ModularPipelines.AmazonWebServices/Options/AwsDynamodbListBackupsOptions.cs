using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dynamodb", "list-backups")]
public record AwsDynamodbListBackupsOptions : AwsOptions
{
    [CliOption("--table-name")]
    public string? TableName { get; set; }

    [CliOption("--time-range-lower-bound")]
    public long? TimeRangeLowerBound { get; set; }

    [CliOption("--time-range-upper-bound")]
    public long? TimeRangeUpperBound { get; set; }

    [CliOption("--backup-type")]
    public string? BackupType { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}