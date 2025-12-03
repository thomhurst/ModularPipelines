using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mobile-network", "pcdp", "create")]
public record AzMobileNetworkPcdpCreateOptions(
[property: CliOption("--access-interface")] string AccessInterface,
[property: CliOption("--name")] string Name,
[property: CliOption("--pccp-name")] string PccpName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}