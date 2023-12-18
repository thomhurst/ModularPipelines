using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functionapp", "deployment", "slot", "create")]
public record AzFunctionappDeploymentSlotCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--slot")] string Slot
) : AzOptions
{
    [CommandSwitch("--configuration-source")]
    public string? ConfigurationSource { get; set; }

    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [CommandSwitch("--registry-password")]
    public string? RegistryPassword { get; set; }

    [CommandSwitch("--registry-username")]
    public string? RegistryUsername { get; set; }
}