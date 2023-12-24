using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("frauddetector", "create-batch-prediction-job")]
public record AwsFrauddetectorCreateBatchPredictionJobOptions(
[property: CommandSwitch("--job-id")] string JobId,
[property: CommandSwitch("--input-path")] string InputPath,
[property: CommandSwitch("--output-path")] string OutputPath,
[property: CommandSwitch("--event-type-name")] string EventTypeName,
[property: CommandSwitch("--detector-name")] string DetectorName,
[property: CommandSwitch("--iam-role-arn")] string IamRoleArn
) : AwsOptions
{
    [CommandSwitch("--detector-version")]
    public string? DetectorVersion { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}