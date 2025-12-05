using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("functionapp", "deployment", "slot", "create")]
public record AzFunctionappDeploymentSlotCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--slot")] string Slot
) : AzOptions
{
    [CliOption("--configuration-source")]
    public string? ConfigurationSource { get; set; }

    [CliOption("--image")]
    public string? Image { get; set; }

    [CliOption("--registry-password")]
    public string? RegistryPassword { get; set; }

    [CliOption("--registry-username")]
    public string? RegistryUsername { get; set; }
}