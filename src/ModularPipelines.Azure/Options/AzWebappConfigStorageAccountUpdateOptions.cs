using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("webapp", "config", "storage-account", "update")]
public record AzWebappConfigStorageAccountUpdateOptions(
[property: CliOption("--custom-id")] string CustomId
) : AzOptions
{
    [CliOption("--access-key")]
    public string? AccessKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--mount-path")]
    public string? MountPath { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--share-name")]
    public string? ShareName { get; set; }

    [CliOption("--slot")]
    public string? Slot { get; set; }

    [CliFlag("--slot-setting")]
    public bool? SlotSetting { get; set; }

    [CliOption("--storage-type")]
    public string? StorageType { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}