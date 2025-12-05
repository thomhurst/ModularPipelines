using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-auto-ml-job")]
public record AwsSagemakerCreateAutoMlJobOptions(
[property: CliOption("--auto-ml-job-name")] string AutoMlJobName,
[property: CliOption("--input-data-config")] string[] InputDataConfig,
[property: CliOption("--output-data-config")] string OutputDataConfig,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--problem-type")]
    public string? ProblemType { get; set; }

    [CliOption("--auto-ml-job-objective")]
    public string? AutoMlJobObjective { get; set; }

    [CliOption("--auto-ml-job-config")]
    public string? AutoMlJobConfig { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--model-deploy-config")]
    public string? ModelDeployConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}