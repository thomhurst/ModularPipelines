using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanroomsml", "delete-training-dataset")]
public record AwsCleanroomsmlDeleteTrainingDatasetOptions(
[property: CliOption("--training-dataset-arn")] string TrainingDatasetArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}