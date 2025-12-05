using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanroomsml", "get-training-dataset")]
public record AwsCleanroomsmlGetTrainingDatasetOptions(
[property: CliOption("--training-dataset-arn")] string TrainingDatasetArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}