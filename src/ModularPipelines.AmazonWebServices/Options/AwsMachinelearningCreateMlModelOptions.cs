using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("machinelearning", "create-ml-model")]
public record AwsMachinelearningCreateMlModelOptions(
[property: CommandSwitch("--ml-model-id")] string MlModelId,
[property: CommandSwitch("--ml-model-type")] string MlModelType,
[property: CommandSwitch("--training-data-source-id")] string TrainingDataSourceId
) : AwsOptions
{
    [CommandSwitch("--ml-model-name")]
    public string? MlModelName { get; set; }

    [CommandSwitch("--parameters")]
    public IEnumerable<KeyValue>? Parameters { get; set; }

    [CommandSwitch("--recipe")]
    public string? Recipe { get; set; }

    [CommandSwitch("--recipe-uri")]
    public string? RecipeUri { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}