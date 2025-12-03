using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dt", "network", "private-endpoint", "connection", "delete")]
public record AzDtNetworkPrivateEndpointConnectionDeleteOptions(
[property: CliOption("--cn")] string Cn,
[property: CliOption("--dt-name")] string DtName
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}