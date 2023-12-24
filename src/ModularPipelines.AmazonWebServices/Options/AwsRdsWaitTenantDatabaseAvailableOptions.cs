using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "wait", "tenant-database-available")]
public record AwsRdsWaitTenantDatabaseAvailableOptions : AwsOptions
{
    [CommandSwitch("--db-instance-identifier")]
    public string? DbInstanceIdentifier { get; set; }

    [CommandSwitch("--tenant-db-name")]
    public string? TenantDbName { get; set; }

    [CommandSwitch("--filters")]
    public string[]? Filters { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}