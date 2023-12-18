using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognitiveservices", "account", "deployment", "create")]
public record AzCognitiveservicesAccountDeploymentCreateOptions(
[property: CommandSwitch("--model-format")] string ModelFormat,
[property: CommandSwitch("--model-name")] string ModelName,
[property: CommandSwitch("--model-version")] string ModelVersion,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--capacity")]
    public string? Capacity { get; set; }

    [CommandSwitch("--deployment-name")]
    public string? DeploymentName { get; set; }

    [CommandSwitch("--model-source")]
    public string? ModelSource { get; set; }

    [CommandSwitch("--scale-capacity")]
    public string? ScaleCapacity { get; set; }

    [CommandSwitch("--scale-settings-scale-type")]
    public string? ScaleSettingsScaleType { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }
}