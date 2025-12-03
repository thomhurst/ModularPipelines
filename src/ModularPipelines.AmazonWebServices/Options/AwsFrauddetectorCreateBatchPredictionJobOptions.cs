using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("frauddetector", "create-batch-prediction-job")]
public record AwsFrauddetectorCreateBatchPredictionJobOptions(
[property: CliOption("--job-id")] string JobId,
[property: CliOption("--input-path")] string InputPath,
[property: CliOption("--output-path")] string OutputPath,
[property: CliOption("--event-type-name")] string EventTypeName,
[property: CliOption("--detector-name")] string DetectorName,
[property: CliOption("--iam-role-arn")] string IamRoleArn
) : AwsOptions
{
    [CliOption("--detector-version")]
    public string? DetectorVersion { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}