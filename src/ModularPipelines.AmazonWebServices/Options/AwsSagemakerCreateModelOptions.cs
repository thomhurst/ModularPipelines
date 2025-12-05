using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-model")]
public record AwsSagemakerCreateModelOptions(
[property: CliOption("--model-name")] string ModelName
) : AwsOptions
{
    [CliOption("--primary-container")]
    public string? PrimaryContainer { get; set; }

    [CliOption("--containers")]
    public string[]? Containers { get; set; }

    [CliOption("--inference-execution-config")]
    public string? InferenceExecutionConfig { get; set; }

    [CliOption("--execution-role-arn")]
    public string? ExecutionRoleArn { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--vpc-config")]
    public string? VpcConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}