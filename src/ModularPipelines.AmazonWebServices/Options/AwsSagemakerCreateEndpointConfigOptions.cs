using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-endpoint-config")]
public record AwsSagemakerCreateEndpointConfigOptions(
[property: CliOption("--endpoint-config-name")] string EndpointConfigName,
[property: CliOption("--production-variants")] string[] ProductionVariants
) : AwsOptions
{
    [CliOption("--data-capture-config")]
    public string? DataCaptureConfig { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--async-inference-config")]
    public string? AsyncInferenceConfig { get; set; }

    [CliOption("--explainer-config")]
    public string? ExplainerConfig { get; set; }

    [CliOption("--shadow-production-variants")]
    public string[]? ShadowProductionVariants { get; set; }

    [CliOption("--execution-role-arn")]
    public string? ExecutionRoleArn { get; set; }

    [CliOption("--vpc-config")]
    public string? VpcConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}