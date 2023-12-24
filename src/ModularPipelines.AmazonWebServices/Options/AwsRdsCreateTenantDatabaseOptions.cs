using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "create-tenant-database")]
public record AwsRdsCreateTenantDatabaseOptions(
[property: CommandSwitch("--db-instance-identifier")] string DbInstanceIdentifier,
[property: CommandSwitch("--tenant-db-name")] string TenantDbName,
[property: CommandSwitch("--master-username")] string MasterUsername,
[property: CommandSwitch("--master-user-password")] string MasterUserPassword
) : AwsOptions
{
    [CommandSwitch("--character-set-name")]
    public string? CharacterSetName { get; set; }

    [CommandSwitch("--nchar-character-set-name")]
    public string? NcharCharacterSetName { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}