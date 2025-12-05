using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-auto-ml-job-v2")]
public record AwsSagemakerCreateAutoMlJobV2Options(
[property: CliOption("--auto-ml-job-name")] string AutoMlJobName,
[property: CliOption("--auto-ml-job-input-data-config")] string[] AutoMlJobInputDataConfig,
[property: CliOption("--output-data-config")] string OutputDataConfig,
[property: CliOption("--auto-ml-problem-type-config")] string AutoMlProblemTypeConfig,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--security-config")]
    public string? SecurityConfig { get; set; }

    [CliOption("--auto-ml-job-objective")]
    public string? AutoMlJobObjective { get; set; }

    [CliOption("--model-deploy-config")]
    public string? ModelDeployConfig { get; set; }

    [CliOption("--data-split-config")]
    public string? DataSplitConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}