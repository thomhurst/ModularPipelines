using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkfabric", "l2domain", "update-admin-state")]
public record AzNetworkfabricL2domainUpdateAdminStateOptions : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-ids")]
    public string? ResourceIds { get; set; }

    [CliOption("--resource-name")]
    public string? ResourceName { get; set; }

    [CliOption("--state")]
    public string? State { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}