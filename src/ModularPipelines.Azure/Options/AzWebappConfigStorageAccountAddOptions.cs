using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("webapp", "config", "storage-account", "add")]
public record AzWebappConfigStorageAccountAddOptions(
[property: CliOption("--access-key")] string AccessKey,
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--custom-id")] string CustomId,
[property: CliOption("--share-name")] string ShareName,
[property: CliOption("--storage-type")] string StorageType
) : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--mount-path")]
    public string? MountPath { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--slot")]
    public string? Slot { get; set; }

    [CliFlag("--slot-setting")]
    public bool? SlotSetting { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}