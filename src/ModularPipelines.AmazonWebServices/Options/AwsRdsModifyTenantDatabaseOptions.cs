using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "modify-tenant-database")]
public record AwsRdsModifyTenantDatabaseOptions(
[property: CommandSwitch("--db-instance-identifier")] string DbInstanceIdentifier,
[property: CommandSwitch("--tenant-db-name")] string TenantDbName
) : AwsOptions
{
    [CommandSwitch("--master-user-password")]
    public string? MasterUserPassword { get; set; }

    [CommandSwitch("--new-tenant-db-name")]
    public string? NewTenantDbName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}