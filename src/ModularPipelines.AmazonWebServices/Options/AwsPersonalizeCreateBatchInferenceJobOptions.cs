using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("personalize", "create-batch-inference-job")]
public record AwsPersonalizeCreateBatchInferenceJobOptions(
[property: CommandSwitch("--job-name")] string JobName,
[property: CommandSwitch("--solution-version-arn")] string SolutionVersionArn,
[property: CommandSwitch("--job-input")] string JobInput,
[property: CommandSwitch("--job-output")] string JobOutput,
[property: CommandSwitch("--role-arn")] string RoleArn
) : AwsOptions
{
    [CommandSwitch("--filter-arn")]
    public string? FilterArn { get; set; }

    [CommandSwitch("--num-results")]
    public int? NumResults { get; set; }

    [CommandSwitch("--batch-inference-job-config")]
    public string? BatchInferenceJobConfig { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--batch-inference-job-mode")]
    public string? BatchInferenceJobMode { get; set; }

    [CommandSwitch("--theme-generation-config")]
    public string? ThemeGenerationConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}