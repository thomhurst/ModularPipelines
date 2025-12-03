using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkcloud", "baremetalmachine", "power-off")]
public record AzNetworkcloudBaremetalmachinePowerOffOptions : AzOptions
{
    [CliOption("--bare-metal-machine-name")]
    public string? BareMetalMachineName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--skip-shutdown")]
    public bool? SkipShutdown { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}