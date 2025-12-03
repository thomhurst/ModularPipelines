using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("machinelearning", "create-ml-model")]
public record AwsMachinelearningCreateMlModelOptions(
[property: CliOption("--ml-model-id")] string MlModelId,
[property: CliOption("--ml-model-type")] string MlModelType,
[property: CliOption("--training-data-source-id")] string TrainingDataSourceId
) : AwsOptions
{
    [CliOption("--ml-model-name")]
    public string? MlModelName { get; set; }

    [CliOption("--parameters")]
    public IEnumerable<KeyValue>? Parameters { get; set; }

    [CliOption("--recipe")]
    public string? Recipe { get; set; }

    [CliOption("--recipe-uri")]
    public string? RecipeUri { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}