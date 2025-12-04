using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "du", "account", "private-endpoint-connection", "show")]
public record AzIotDuAccountPrivateEndpointConnectionShowOptions(
[property: CliOption("--account")] int Account,
[property: CliOption("--cn")] string Cn
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}