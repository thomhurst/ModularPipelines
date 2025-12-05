using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("acr", "config", "authentication-as-arm", "update")]
public record AzAcrConfigAuthenticationAsArmUpdateOptions(
[property: CliOption("--registry")] string Registry
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }
}