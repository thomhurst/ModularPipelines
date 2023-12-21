using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dla", "account", "blob-storage", "update")]
public record AzDlaAccountBlobStorageUpdateOptions(
[property: CommandSwitch("--access-key")] string AccessKey,
[property: CommandSwitch("--storage-account-name")] int StorageAccountName
) : AzOptions
{
    [CommandSwitch("--account")]
    public int? Account { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--suffix")]
    public string? Suffix { get; set; }
}