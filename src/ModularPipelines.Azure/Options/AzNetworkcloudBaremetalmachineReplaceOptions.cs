using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkcloud", "baremetalmachine", "replace")]
public record AzNetworkcloudBaremetalmachineReplaceOptions : AzOptions
{
    [CliOption("--bare-metal-machine-name")]
    public string? BareMetalMachineName { get; set; }

    [CliOption("--bmc-credentials")]
    public string? BmcCredentials { get; set; }

    [CliOption("--bmc-mac-address")]
    public string? BmcMacAddress { get; set; }

    [CliOption("--boot-mac-address")]
    public string? BootMacAddress { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--machine-name")]
    public string? MachineName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--serial-number")]
    public string? SerialNumber { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}