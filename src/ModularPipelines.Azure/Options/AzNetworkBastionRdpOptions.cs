using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "bastion", "rdp")]
public record AzNetworkBastionRdpOptions : AzOptions
{
    [CliOption("--auth-type")]
    public string? AuthType { get; set; }

    [CliFlag("--configure")]
    public bool? Configure { get; set; }

    [CliFlag("--disable-gateway")]
    public bool? DisableGateway { get; set; }

    [CliFlag("--enable-mfa")]
    public bool? EnableMfa { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-port")]
    public string? ResourcePort { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--target-ip-address")]
    public string? TargetIpAddress { get; set; }

    [CliOption("--target-resource-id")]
    public string? TargetResourceId { get; set; }
}