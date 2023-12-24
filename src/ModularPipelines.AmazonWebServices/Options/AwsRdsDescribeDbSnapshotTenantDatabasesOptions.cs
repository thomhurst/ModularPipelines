using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "describe-db-snapshot-tenant-databases")]
public record AwsRdsDescribeDbSnapshotTenantDatabasesOptions : AwsOptions
{
    [CommandSwitch("--db-instance-identifier")]
    public string? DbInstanceIdentifier { get; set; }

    [CommandSwitch("--db-snapshot-identifier")]
    public string? DbSnapshotIdentifier { get; set; }

    [CommandSwitch("--snapshot-type")]
    public string? SnapshotType { get; set; }

    [CommandSwitch("--filters")]
    public string[]? Filters { get; set; }

    [CommandSwitch("--dbi-resource-id")]
    public string? DbiResourceId { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}