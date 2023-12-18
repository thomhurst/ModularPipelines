using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "account", "outbound-endpoints")]
public record AzBatchAccountOutboundEndpointsOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--encryption-key-identifier")]
    public string? EncryptionKeyIdentifier { get; set; }

    [CommandSwitch("--encryption-key-source")]
    public string? EncryptionKeySource { get; set; }

    [CommandSwitch("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CommandSwitch("--storage-account")]
    public int? StorageAccount { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}