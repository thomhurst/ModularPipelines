using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-model")]
public record AwsSagemakerCreateModelOptions(
[property: CommandSwitch("--model-name")] string ModelName
) : AwsOptions
{
    [CommandSwitch("--primary-container")]
    public string? PrimaryContainer { get; set; }

    [CommandSwitch("--containers")]
    public string[]? Containers { get; set; }

    [CommandSwitch("--inference-execution-config")]
    public string? InferenceExecutionConfig { get; set; }

    [CommandSwitch("--execution-role-arn")]
    public string? ExecutionRoleArn { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--vpc-config")]
    public string? VpcConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}