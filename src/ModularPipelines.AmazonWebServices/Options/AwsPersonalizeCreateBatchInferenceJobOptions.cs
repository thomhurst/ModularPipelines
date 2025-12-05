using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("personalize", "create-batch-inference-job")]
public record AwsPersonalizeCreateBatchInferenceJobOptions(
[property: CliOption("--job-name")] string JobName,
[property: CliOption("--solution-version-arn")] string SolutionVersionArn,
[property: CliOption("--job-input")] string JobInput,
[property: CliOption("--job-output")] string JobOutput,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--filter-arn")]
    public string? FilterArn { get; set; }

    [CliOption("--num-results")]
    public int? NumResults { get; set; }

    [CliOption("--batch-inference-job-config")]
    public string? BatchInferenceJobConfig { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--batch-inference-job-mode")]
    public string? BatchInferenceJobMode { get; set; }

    [CliOption("--theme-generation-config")]
    public string? ThemeGenerationConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}