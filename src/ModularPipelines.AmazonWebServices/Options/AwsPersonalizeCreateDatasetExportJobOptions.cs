using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("personalize", "create-dataset-export-job")]
public record AwsPersonalizeCreateDatasetExportJobOptions(
[property: CliOption("--job-name")] string JobName,
[property: CliOption("--dataset-arn")] string DatasetArn,
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--job-output")] string JobOutput
) : AwsOptions
{
    [CliOption("--ingestion-mode")]
    public string? IngestionMode { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}