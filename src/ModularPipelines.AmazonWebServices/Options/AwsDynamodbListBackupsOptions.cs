using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dynamodb", "list-backups")]
public record AwsDynamodbListBackupsOptions : AwsOptions
{
    [CommandSwitch("--table-name")]
    public string? TableName { get; set; }

    [CommandSwitch("--time-range-lower-bound")]
    public long? TimeRangeLowerBound { get; set; }

    [CommandSwitch("--time-range-upper-bound")]
    public long? TimeRangeUpperBound { get; set; }

    [CommandSwitch("--backup-type")]
    public string? BackupType { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}