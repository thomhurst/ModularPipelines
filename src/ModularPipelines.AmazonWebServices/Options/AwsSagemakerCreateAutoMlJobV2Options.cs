using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-auto-ml-job-v2")]
public record AwsSagemakerCreateAutoMlJobV2Options(
[property: CommandSwitch("--auto-ml-job-name")] string AutoMlJobName,
[property: CommandSwitch("--auto-ml-job-input-data-config")] string[] AutoMlJobInputDataConfig,
[property: CommandSwitch("--output-data-config")] string OutputDataConfig,
[property: CommandSwitch("--auto-ml-problem-type-config")] string AutoMlProblemTypeConfig,
[property: CommandSwitch("--role-arn")] string RoleArn
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--security-config")]
    public string? SecurityConfig { get; set; }

    [CommandSwitch("--auto-ml-job-objective")]
    public string? AutoMlJobObjective { get; set; }

    [CommandSwitch("--model-deploy-config")]
    public string? ModelDeployConfig { get; set; }

    [CommandSwitch("--data-split-config")]
    public string? DataSplitConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}