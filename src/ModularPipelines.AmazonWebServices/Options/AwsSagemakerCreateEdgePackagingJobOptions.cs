using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-edge-packaging-job")]
public record AwsSagemakerCreateEdgePackagingJobOptions(
[property: CliOption("--edge-packaging-job-name")] string EdgePackagingJobName,
[property: CliOption("--compilation-job-name")] string CompilationJobName,
[property: CliOption("--model-name")] string ModelName,
[property: CliOption("--model-version")] string ModelVersion,
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--output-config")] string OutputConfig
) : AwsOptions
{
    [CliOption("--resource-key")]
    public string? ResourceKey { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}