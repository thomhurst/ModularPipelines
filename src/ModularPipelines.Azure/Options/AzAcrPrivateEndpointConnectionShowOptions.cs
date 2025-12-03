using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acr", "private-endpoint-connection", "show")]
public record AzAcrPrivateEndpointConnectionShowOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--registry-name")] string RegistryName
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}