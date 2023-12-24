using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-endpoint-config")]
public record AwsSagemakerCreateEndpointConfigOptions(
[property: CommandSwitch("--endpoint-config-name")] string EndpointConfigName,
[property: CommandSwitch("--production-variants")] string[] ProductionVariants
) : AwsOptions
{
    [CommandSwitch("--data-capture-config")]
    public string? DataCaptureConfig { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--async-inference-config")]
    public string? AsyncInferenceConfig { get; set; }

    [CommandSwitch("--explainer-config")]
    public string? ExplainerConfig { get; set; }

    [CommandSwitch("--shadow-production-variants")]
    public string[]? ShadowProductionVariants { get; set; }

    [CommandSwitch("--execution-role-arn")]
    public string? ExecutionRoleArn { get; set; }

    [CommandSwitch("--vpc-config")]
    public string? VpcConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}