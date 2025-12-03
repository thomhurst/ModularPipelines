using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dt", "network", "private-endpoint", "connection", "show")]
public record AzDtNetworkPrivateEndpointConnectionShowOptions(
[property: CliOption("--cn")] string Cn,
[property: CliOption("--dt-name")] string DtName
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}