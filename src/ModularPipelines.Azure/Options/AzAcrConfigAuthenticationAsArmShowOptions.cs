using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("acr", "config", "authentication-as-arm", "show")]
public record AzAcrConfigAuthenticationAsArmShowOptions(
[property: CliOption("--registry")] string Registry
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}