using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutequipment", "create-model")]
public record AwsLookoutequipmentCreateModelOptions(
[property: CliOption("--model-name")] string ModelName,
[property: CliOption("--dataset-name")] string DatasetName
) : AwsOptions
{
    [CliOption("--dataset-schema")]
    public string? DatasetSchema { get; set; }

    [CliOption("--labels-input-configuration")]
    public string? LabelsInputConfiguration { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--training-data-start-time")]
    public long? TrainingDataStartTime { get; set; }

    [CliOption("--training-data-end-time")]
    public long? TrainingDataEndTime { get; set; }

    [CliOption("--evaluation-data-start-time")]
    public long? EvaluationDataStartTime { get; set; }

    [CliOption("--evaluation-data-end-time")]
    public long? EvaluationDataEndTime { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--data-pre-processing-configuration")]
    public string? DataPreProcessingConfiguration { get; set; }

    [CliOption("--server-side-kms-key-id")]
    public string? ServerSideKmsKeyId { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--off-condition")]
    public string? OffCondition { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}