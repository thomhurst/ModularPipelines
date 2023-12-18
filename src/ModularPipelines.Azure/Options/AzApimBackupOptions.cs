using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apim", "backup")]
public record AzApimBackupOptions(
[property: CommandSwitch("--backup-name")] string BackupName,
[property: CommandSwitch("--container-name")] string ContainerName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--storage-account-key")] int StorageAccountKey,
[property: CommandSwitch("--storage-account-name")] int StorageAccountName
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}