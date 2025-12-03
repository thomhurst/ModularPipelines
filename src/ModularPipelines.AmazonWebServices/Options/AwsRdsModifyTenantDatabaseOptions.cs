using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "modify-tenant-database")]
public record AwsRdsModifyTenantDatabaseOptions(
[property: CliOption("--db-instance-identifier")] string DbInstanceIdentifier,
[property: CliOption("--tenant-db-name")] string TenantDbName
) : AwsOptions
{
    [CliOption("--master-user-password")]
    public string? MasterUserPassword { get; set; }

    [CliOption("--new-tenant-db-name")]
    public string? NewTenantDbName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}