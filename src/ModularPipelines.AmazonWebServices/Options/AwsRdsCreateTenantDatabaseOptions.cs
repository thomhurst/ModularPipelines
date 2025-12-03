using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "create-tenant-database")]
public record AwsRdsCreateTenantDatabaseOptions(
[property: CliOption("--db-instance-identifier")] string DbInstanceIdentifier,
[property: CliOption("--tenant-db-name")] string TenantDbName,
[property: CliOption("--master-username")] string MasterUsername,
[property: CliOption("--master-user-password")] string MasterUserPassword
) : AwsOptions
{
    [CliOption("--character-set-name")]
    public string? CharacterSetName { get; set; }

    [CliOption("--nchar-character-set-name")]
    public string? NcharCharacterSetName { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}