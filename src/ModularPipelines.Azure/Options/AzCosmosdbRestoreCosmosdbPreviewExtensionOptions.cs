using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "restore", "(cosmosdb-preview", "extension)")]
public record AzCosmosdbRestoreCosmosdbPreviewExtensionOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--location")] string Location,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--restore-timestamp")] string RestoreTimestamp,
[property: CliOption("--target-database-account-name")] int TargetDatabaseAccountName
) : AzOptions
{
    [CliOption("--assign-identity")]
    public string? AssignIdentity { get; set; }

    [CliOption("--databases-to-restore")]
    public string? DatabasesToRestore { get; set; }

    [CliOption("--default-identity")]
    public string? DefaultIdentity { get; set; }

    [CliFlag("--enable-public-network")]
    public bool? EnablePublicNetwork { get; set; }

    [CliOption("--gremlin-databases-to-restore")]
    public string? GremlinDatabasesToRestore { get; set; }

    [CliOption("--source-backup-location")]
    public string? SourceBackupLocation { get; set; }

    [CliOption("--tables-to-restore")]
    public string? TablesToRestore { get; set; }
}