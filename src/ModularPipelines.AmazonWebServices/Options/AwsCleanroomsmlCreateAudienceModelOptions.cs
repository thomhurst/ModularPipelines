using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanroomsml", "create-audience-model")]
public record AwsCleanroomsmlCreateAudienceModelOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--training-dataset-arn")] string TrainingDatasetArn
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--kms-key-arn")]
    public string? KmsKeyArn { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--training-data-end-time")]
    public long? TrainingDataEndTime { get; set; }

    [CliOption("--training-data-start-time")]
    public long? TrainingDataStartTime { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}