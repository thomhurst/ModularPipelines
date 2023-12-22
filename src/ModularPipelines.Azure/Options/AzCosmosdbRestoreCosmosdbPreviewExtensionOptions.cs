using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "restore", "(cosmosdb-preview", "extension)")]
public record AzCosmosdbRestoreCosmosdbPreviewExtensionOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--restore-timestamp")] string RestoreTimestamp,
[property: CommandSwitch("--target-database-account-name")] int TargetDatabaseAccountName
) : AzOptions
{
    [CommandSwitch("--assign-identity")]
    public string? AssignIdentity { get; set; }

    [CommandSwitch("--databases-to-restore")]
    public string? DatabasesToRestore { get; set; }

    [CommandSwitch("--default-identity")]
    public string? DefaultIdentity { get; set; }

    [BooleanCommandSwitch("--enable-public-network")]
    public bool? EnablePublicNetwork { get; set; }

    [CommandSwitch("--gremlin-databases-to-restore")]
    public string? GremlinDatabasesToRestore { get; set; }

    [CommandSwitch("--source-backup-location")]
    public string? SourceBackupLocation { get; set; }

    [CommandSwitch("--tables-to-restore")]
    public string? TablesToRestore { get; set; }
}