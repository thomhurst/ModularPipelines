using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-model-package")]
public record AwsSagemakerCreateModelPackageOptions : AwsOptions
{
    [CommandSwitch("--model-package-name")]
    public string? ModelPackageName { get; set; }

    [CommandSwitch("--model-package-group-name")]
    public string? ModelPackageGroupName { get; set; }

    [CommandSwitch("--model-package-description")]
    public string? ModelPackageDescription { get; set; }

    [CommandSwitch("--inference-specification")]
    public string? InferenceSpecification { get; set; }

    [CommandSwitch("--validation-specification")]
    public string? ValidationSpecification { get; set; }

    [CommandSwitch("--source-algorithm-specification")]
    public string? SourceAlgorithmSpecification { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--model-approval-status")]
    public string? ModelApprovalStatus { get; set; }

    [CommandSwitch("--metadata-properties")]
    public string? MetadataProperties { get; set; }

    [CommandSwitch("--model-metrics")]
    public string? ModelMetrics { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--domain")]
    public string? Domain { get; set; }

    [CommandSwitch("--task")]
    public string? Task { get; set; }

    [CommandSwitch("--sample-payload-url")]
    public string? SamplePayloadUrl { get; set; }

    [CommandSwitch("--customer-metadata-properties")]
    public IEnumerable<KeyValue>? CustomerMetadataProperties { get; set; }

    [CommandSwitch("--drift-check-baselines")]
    public string? DriftCheckBaselines { get; set; }

    [CommandSwitch("--additional-inference-specifications")]
    public string[]? AdditionalInferenceSpecifications { get; set; }

    [CommandSwitch("--skip-model-validation")]
    public string? SkipModelValidation { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}