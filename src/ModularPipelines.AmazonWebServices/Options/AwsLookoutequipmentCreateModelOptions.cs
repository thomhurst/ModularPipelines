using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutequipment", "create-model")]
public record AwsLookoutequipmentCreateModelOptions(
[property: CommandSwitch("--model-name")] string ModelName,
[property: CommandSwitch("--dataset-name")] string DatasetName
) : AwsOptions
{
    [CommandSwitch("--dataset-schema")]
    public string? DatasetSchema { get; set; }

    [CommandSwitch("--labels-input-configuration")]
    public string? LabelsInputConfiguration { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--training-data-start-time")]
    public long? TrainingDataStartTime { get; set; }

    [CommandSwitch("--training-data-end-time")]
    public long? TrainingDataEndTime { get; set; }

    [CommandSwitch("--evaluation-data-start-time")]
    public long? EvaluationDataStartTime { get; set; }

    [CommandSwitch("--evaluation-data-end-time")]
    public long? EvaluationDataEndTime { get; set; }

    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--data-pre-processing-configuration")]
    public string? DataPreProcessingConfiguration { get; set; }

    [CommandSwitch("--server-side-kms-key-id")]
    public string? ServerSideKmsKeyId { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--off-condition")]
    public string? OffCondition { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}