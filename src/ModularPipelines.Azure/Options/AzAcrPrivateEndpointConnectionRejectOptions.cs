using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("acr", "private-endpoint-connection", "reject")]
public record AzAcrPrivateEndpointConnectionRejectOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--registry-name")] string RegistryName
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}