using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-compilation-job")]
public record AwsSagemakerCreateCompilationJobOptions(
[property: CommandSwitch("--compilation-job-name")] string CompilationJobName,
[property: CommandSwitch("--role-arn")] string RoleArn,
[property: CommandSwitch("--output-config")] string OutputConfig,
[property: CommandSwitch("--stopping-condition")] string StoppingCondition
) : AwsOptions
{
    [CommandSwitch("--model-package-version-arn")]
    public string? ModelPackageVersionArn { get; set; }

    [CommandSwitch("--input-config")]
    public string? InputConfig { get; set; }

    [CommandSwitch("--vpc-config")]
    public string? VpcConfig { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}