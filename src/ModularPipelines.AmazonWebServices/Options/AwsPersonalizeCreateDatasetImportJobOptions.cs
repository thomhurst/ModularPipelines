using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("personalize", "create-dataset-import-job")]
public record AwsPersonalizeCreateDatasetImportJobOptions(
[property: CliOption("--job-name")] string JobName,
[property: CliOption("--dataset-arn")] string DatasetArn,
[property: CliOption("--data-source")] string DataSource,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--import-mode")]
    public string? ImportMode { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}