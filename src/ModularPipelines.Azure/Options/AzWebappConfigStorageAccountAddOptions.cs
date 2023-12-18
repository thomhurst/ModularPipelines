using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "config", "storage-account", "add")]
public record AzWebappConfigStorageAccountAddOptions(
[property: CommandSwitch("--access-key")] string AccessKey,
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--custom-id")] string CustomId,
[property: CommandSwitch("--share-name")] string ShareName,
[property: CommandSwitch("--storage-type")] string StorageType
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--mount-path")]
    public string? MountPath { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--slot")]
    public string? Slot { get; set; }

    [BooleanCommandSwitch("--slot-setting")]
    public bool? SlotSetting { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}

