using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ams", "account", "storage", "set-authentication")]
public record AzAmsAccountStorageSetAuthenticationOptions(
[property: CommandSwitch("--storage-auth")] string StorageAuth
) : AzOptions
{
    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--storage-account-id")]
    public int? StorageAccountId { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [BooleanCommandSwitch("--system-assigned")]
    public bool? SystemAssigned { get; set; }

    [CommandSwitch("--user-assigned")]
    public string? UserAssigned { get; set; }
}