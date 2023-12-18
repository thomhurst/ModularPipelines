using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "connected-env", "storage", "set")]
public record AzContainerappConnectedEnvStorageSetOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--storage-name")] string StorageName
) : AzOptions
{
    [CommandSwitch("--access-mode")]
    public string? AccessMode { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--azure-file-account-key")]
    public int? AzureFileAccountKey { get; set; }

    [CommandSwitch("--azure-file-share-name")]
    public string? AzureFileShareName { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}

