using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkcloud", "baremetalmachine", "cordon")]
public record AzNetworkcloudBaremetalmachineCordonOptions : AzOptions
{
    [CliOption("--bare-metal-machine-name")]
    public string? BareMetalMachineName { get; set; }

    [CliFlag("--evacuate")]
    public bool? Evacuate { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}