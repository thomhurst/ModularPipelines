using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "private-endpoint", "asg", "add")]
public record AzNetworkPrivateEndpointAsgAddOptions(
[property: CliOption("--endpoint-name")] string EndpointName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--asg-id")]
    public string? AsgId { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}