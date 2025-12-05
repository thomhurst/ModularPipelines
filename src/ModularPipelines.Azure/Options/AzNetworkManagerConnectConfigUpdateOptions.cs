using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "manager", "connect-config", "update")]
public record AzNetworkManagerConnectConfigUpdateOptions : AzOptions
{
    [CliOption("--applies-to-groups")]
    public string? AppliesToGroups { get; set; }

    [CliOption("--configuration-name")]
    public string? ConfigurationName { get; set; }

    [CliFlag("--delete-existing-peering")]
    public bool? DeleteExistingPeering { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--hub")]
    public string? Hub { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--is-global")]
    public bool? IsGlobal { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}