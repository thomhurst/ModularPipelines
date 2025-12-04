using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "du", "account", "private-endpoint-connection", "set")]
public record AzIotDuAccountPrivateEndpointConnectionSetOptions(
[property: CliOption("--account")] int Account,
[property: CliOption("--cn")] string Cn,
[property: CliOption("--status")] string Status
) : AzOptions
{
    [CliOption("--desc")]
    public string? Desc { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}