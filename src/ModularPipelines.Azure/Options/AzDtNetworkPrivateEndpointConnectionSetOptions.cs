using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dt", "network", "private-endpoint", "connection", "set")]
public record AzDtNetworkPrivateEndpointConnectionSetOptions(
[property: CliOption("--cn")] string Cn,
[property: CliOption("--dt-name")] string DtName,
[property: CliOption("--status")] string Status
) : AzOptions
{
    [CliOption("--actions-required")]
    public string? ActionsRequired { get; set; }

    [CliOption("--desc")]
    public string? Desc { get; set; }

    [CliOption("--group-ids")]
    public string? GroupIds { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}