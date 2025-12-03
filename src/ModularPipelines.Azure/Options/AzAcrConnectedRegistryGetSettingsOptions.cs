using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acr", "connected-registry", "get-settings")]
public record AzAcrConnectedRegistryGetSettingsOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--parent-protocol")] string ParentProtocol,
[property: CliOption("--registry")] string Registry
) : AzOptions
{
    [CliOption("--generate-password")]
    public string? GeneratePassword { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}