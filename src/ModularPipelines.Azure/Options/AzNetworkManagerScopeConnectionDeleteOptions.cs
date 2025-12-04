using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "manager", "scope-connection", "delete")]
public record AzNetworkManagerScopeConnectionDeleteOptions : AzOptions
{
    [CliOption("--connection-name")]
    public string? ConnectionName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--network-manager")]
    public string? NetworkManager { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}