using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("keyvault", "private-endpoint-connection", "list")]
public record AzKeyvaultPrivateEndpointConnectionListOptions(
[property: CliOption("--hsm-name")] string HsmName
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}