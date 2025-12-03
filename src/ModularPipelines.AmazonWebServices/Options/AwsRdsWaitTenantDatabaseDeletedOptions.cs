using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "wait", "tenant-database-deleted")]
public record AwsRdsWaitTenantDatabaseDeletedOptions : AwsOptions
{
    [CliOption("--db-instance-identifier")]
    public string? DbInstanceIdentifier { get; set; }

    [CliOption("--tenant-db-name")]
    public string? TenantDbName { get; set; }

    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}