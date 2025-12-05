using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "account", "network-rule", "remove")]
public record AzStorageAccountNetworkRuleRemoveOptions(
[property: CliOption("--account-name")] int AccountName
) : AzOptions
{
    [CliOption("--ip-address")]
    public string? IpAddress { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-id")]
    public string? ResourceId { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--tenant-id")]
    public string? TenantId { get; set; }

    [CliOption("--vnet-name")]
    public string? VnetName { get; set; }
}