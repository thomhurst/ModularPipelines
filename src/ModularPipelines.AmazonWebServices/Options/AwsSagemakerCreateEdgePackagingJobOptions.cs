using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-edge-packaging-job")]
public record AwsSagemakerCreateEdgePackagingJobOptions(
[property: CommandSwitch("--edge-packaging-job-name")] string EdgePackagingJobName,
[property: CommandSwitch("--compilation-job-name")] string CompilationJobName,
[property: CommandSwitch("--model-name")] string ModelName,
[property: CommandSwitch("--model-version")] string ModelVersion,
[property: CommandSwitch("--role-arn")] string RoleArn,
[property: CommandSwitch("--output-config")] string OutputConfig
) : AwsOptions
{
    [CommandSwitch("--resource-key")]
    public string? ResourceKey { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}