using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-compilation-job")]
public record AwsSagemakerCreateCompilationJobOptions(
[property: CliOption("--compilation-job-name")] string CompilationJobName,
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--output-config")] string OutputConfig,
[property: CliOption("--stopping-condition")] string StoppingCondition
) : AwsOptions
{
    [CliOption("--model-package-version-arn")]
    public string? ModelPackageVersionArn { get; set; }

    [CliOption("--input-config")]
    public string? InputConfig { get; set; }

    [CliOption("--vpc-config")]
    public string? VpcConfig { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}