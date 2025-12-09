using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("frauddetector", "create-batch-import-job")]
public record AwsFrauddetectorCreateBatchImportJobOptions(
[property: CliOption("--job-id")] string JobId,
[property: CliOption("--input-path")] string InputPath,
[property: CliOption("--output-path")] string OutputPath,
[property: CliOption("--event-type-name")] string EventTypeName,
[property: CliOption("--iam-role-arn")] string IamRoleArn
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}