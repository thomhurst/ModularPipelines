using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-auto-ml-job")]
public record AwsSagemakerCreateAutoMlJobOptions(
[property: CommandSwitch("--auto-ml-job-name")] string AutoMlJobName,
[property: CommandSwitch("--input-data-config")] string[] InputDataConfig,
[property: CommandSwitch("--output-data-config")] string OutputDataConfig,
[property: CommandSwitch("--role-arn")] string RoleArn
) : AwsOptions
{
    [CommandSwitch("--problem-type")]
    public string? ProblemType { get; set; }

    [CommandSwitch("--auto-ml-job-objective")]
    public string? AutoMlJobObjective { get; set; }

    [CommandSwitch("--auto-ml-job-config")]
    public string? AutoMlJobConfig { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--model-deploy-config")]
    public string? ModelDeployConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}