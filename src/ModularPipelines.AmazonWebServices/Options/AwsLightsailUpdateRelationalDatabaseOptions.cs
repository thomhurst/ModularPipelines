using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "update-relational-database")]
public record AwsLightsailUpdateRelationalDatabaseOptions(
[property: CliOption("--relational-database-name")] string RelationalDatabaseName
) : AwsOptions
{
    [CliOption("--master-user-password")]
    public string? MasterUserPassword { get; set; }

    [CliOption("--preferred-backup-window")]
    public string? PreferredBackupWindow { get; set; }

    [CliOption("--preferred-maintenance-window")]
    public string? PreferredMaintenanceWindow { get; set; }

    [CliOption("--ca-certificate-identifier")]
    public string? CaCertificateIdentifier { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}