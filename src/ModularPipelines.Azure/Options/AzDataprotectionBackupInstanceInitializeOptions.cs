using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataprotection", "backup-instance", "initialize")]
public record AzDataprotectionBackupInstanceInitializeOptions(
[property: CommandSwitch("--datasource-id")] string DatasourceId,
[property: CommandSwitch("--datasource-location")] string DatasourceLocation,
[property: CommandSwitch("--datasource-type")] string DatasourceType,
[property: CommandSwitch("--policy-id")] string PolicyId
) : AzOptions
{
    [CommandSwitch("--backup-configuration")]
    public string? BackupConfiguration { get; set; }

    [CommandSwitch("--friendly-name")]
    public string? FriendlyName { get; set; }

    [CommandSwitch("--secret-store-type")]
    public string? SecretStoreType { get; set; }

    [CommandSwitch("--secret-store-uri")]
    public string? SecretStoreUri { get; set; }

    [CommandSwitch("--snapshot-resource-group-name")]
    public string? SnapshotResourceGroupName { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

