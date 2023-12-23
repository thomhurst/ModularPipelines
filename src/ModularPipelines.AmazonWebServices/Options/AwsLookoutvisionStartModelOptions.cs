using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutvision", "start-model")]
public record AwsLookoutvisionStartModelOptions(
[property: CommandSwitch("--project-name")] string ProjectName,
[property: CommandSwitch("--model-version")] string ModelVersion,
[property: CommandSwitch("--min-inference-units")] int MinInferenceUnits
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--max-inference-units")]
    public int? MaxInferenceUnits { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}