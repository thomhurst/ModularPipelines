using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "private-endpoint", "ip-config", "add")]
public record AzNetworkPrivateEndpointIpConfigAddOptions(
[property: CliOption("--endpoint-name")] string EndpointName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--group-id")]
    public string? GroupId { get; set; }

    [CliOption("--member-name")]
    public string? MemberName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--private-ip-address")]
    public string? PrivateIpAddress { get; set; }
}