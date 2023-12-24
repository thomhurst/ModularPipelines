using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cleanroomsml", "create-audience-model")]
public record AwsCleanroomsmlCreateAudienceModelOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--training-dataset-arn")] string TrainingDatasetArn
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--kms-key-arn")]
    public string? KmsKeyArn { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--training-data-end-time")]
    public long? TrainingDataEndTime { get; set; }

    [CommandSwitch("--training-data-start-time")]
    public long? TrainingDataStartTime { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}