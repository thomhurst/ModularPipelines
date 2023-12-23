using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cleanroomsml", "delete-training-dataset")]
public record AwsCleanroomsmlDeleteTrainingDatasetOptions(
[property: CommandSwitch("--training-dataset-arn")] string TrainingDatasetArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}