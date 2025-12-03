using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "du", "account", "private-endpoint-connection", "delete")]
public record AzIotDuAccountPrivateEndpointConnectionDeleteOptions(
[property: CliOption("--account")] int Account,
[property: CliOption("--cn")] string Cn
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}