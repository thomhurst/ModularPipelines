using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "account", "network-rule", "add")]
public record AzStorageAccountNetworkRuleAddOptions(
[property: CommandSwitch("--account-name")] int AccountName
) : AzOptions
{
    [CommandSwitch("--action")]
    public string? Action { get; set; }

    [CommandSwitch("--ip-address")]
    public string? IpAddress { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--resource-id")]
    public string? ResourceId { get; set; }

    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }

    [CommandSwitch("--tenant-id")]
    public string? TenantId { get; set; }

    [CommandSwitch("--vnet-name")]
    public string? VnetName { get; set; }
}