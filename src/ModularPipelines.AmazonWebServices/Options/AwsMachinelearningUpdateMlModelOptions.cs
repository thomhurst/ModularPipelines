using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("machinelearning", "update-ml-model")]
public record AwsMachinelearningUpdateMlModelOptions(
[property: CommandSwitch("--ml-model-id")] string MlModelId
) : AwsOptions
{
    [CommandSwitch("--ml-model-name")]
    public string? MlModelName { get; set; }

    [CommandSwitch("--score-threshold")]
    public float? ScoreThreshold { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}