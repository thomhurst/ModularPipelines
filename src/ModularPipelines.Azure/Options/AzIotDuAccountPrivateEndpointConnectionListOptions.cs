using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "du", "account", "private-endpoint-connection", "list")]
public record AzIotDuAccountPrivateEndpointConnectionListOptions(
[property: CliOption("--account")] int Account
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}