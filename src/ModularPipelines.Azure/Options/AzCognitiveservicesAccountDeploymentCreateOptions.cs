using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cognitiveservices", "account", "deployment", "create")]
public record AzCognitiveservicesAccountDeploymentCreateOptions(
[property: CliOption("--model-format")] string ModelFormat,
[property: CliOption("--model-name")] string ModelName,
[property: CliOption("--model-version")] string ModelVersion,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--capacity")]
    public string? Capacity { get; set; }

    [CliOption("--deployment-name")]
    public string? DeploymentName { get; set; }

    [CliOption("--model-source")]
    public string? ModelSource { get; set; }

    [CliOption("--scale-capacity")]
    public string? ScaleCapacity { get; set; }

    [CliOption("--scale-settings-scale-type")]
    public string? ScaleSettingsScaleType { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }
}