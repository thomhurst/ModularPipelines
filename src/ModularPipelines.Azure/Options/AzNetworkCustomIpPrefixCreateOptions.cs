using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "custom-ip", "prefix", "create")]
public record AzNetworkCustomIpPrefixCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--asn")]
    public string? Asn { get; set; }

    [CliOption("--authorization-message")]
    public string? AuthorizationMessage { get; set; }

    [CliOption("--cidr")]
    public string? Cidr { get; set; }

    [CliOption("--cip-prefix-parent")]
    public string? CipPrefixParent { get; set; }

    [CliFlag("--express-route-advertise")]
    public bool? ExpressRouteAdvertise { get; set; }

    [CliOption("--geo")]
    public string? Geo { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--signed-message")]
    public string? SignedMessage { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}