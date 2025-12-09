using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanroomsml", "create-training-dataset")]
public record AwsCleanroomsmlCreateTrainingDatasetOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--training-data")] string[] TrainingData
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}