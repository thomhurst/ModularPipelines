using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "bastion", "tunnel")]
public record AzNetworkBastionTunnelOptions(
[property: CliOption("--port")] int Port,
[property: CliOption("--resource-port")] string ResourcePort
) : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--target-ip-address")]
    public string? TargetIpAddress { get; set; }

    [CliOption("--target-resource-id")]
    public string? TargetResourceId { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }
}