using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "config", "storage-account", "update")]
public record AzWebappConfigStorageAccountUpdateOptions(
[property: CommandSwitch("--custom-id")] string CustomId
) : AzOptions
{
    [CommandSwitch("--access-key")]
    public string? AccessKey { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--mount-path")]
    public string? MountPath { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--share-name")]
    public string? ShareName { get; set; }

    [CommandSwitch("--slot")]
    public string? Slot { get; set; }

    [BooleanCommandSwitch("--slot-setting")]
    public bool? SlotSetting { get; set; }

    [CommandSwitch("--storage-type")]
    public string? StorageType { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}

