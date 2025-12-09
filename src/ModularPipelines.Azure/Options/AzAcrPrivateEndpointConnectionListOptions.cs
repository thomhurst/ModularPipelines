using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("acr", "private-endpoint-connection", "list")]
public record AzAcrPrivateEndpointConnectionListOptions(
[property: CliOption("--registry-name")] string RegistryName
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}