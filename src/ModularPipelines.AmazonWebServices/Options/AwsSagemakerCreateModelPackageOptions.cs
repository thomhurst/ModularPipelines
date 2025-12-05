using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-model-package")]
public record AwsSagemakerCreateModelPackageOptions : AwsOptions
{
    [CliOption("--model-package-name")]
    public string? ModelPackageName { get; set; }

    [CliOption("--model-package-group-name")]
    public string? ModelPackageGroupName { get; set; }

    [CliOption("--model-package-description")]
    public string? ModelPackageDescription { get; set; }

    [CliOption("--inference-specification")]
    public string? InferenceSpecification { get; set; }

    [CliOption("--validation-specification")]
    public string? ValidationSpecification { get; set; }

    [CliOption("--source-algorithm-specification")]
    public string? SourceAlgorithmSpecification { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--model-approval-status")]
    public string? ModelApprovalStatus { get; set; }

    [CliOption("--metadata-properties")]
    public string? MetadataProperties { get; set; }

    [CliOption("--model-metrics")]
    public string? ModelMetrics { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--domain")]
    public string? Domain { get; set; }

    [CliOption("--task")]
    public string? Task { get; set; }

    [CliOption("--sample-payload-url")]
    public string? SamplePayloadUrl { get; set; }

    [CliOption("--customer-metadata-properties")]
    public IEnumerable<KeyValue>? CustomerMetadataProperties { get; set; }

    [CliOption("--drift-check-baselines")]
    public string? DriftCheckBaselines { get; set; }

    [CliOption("--additional-inference-specifications")]
    public string[]? AdditionalInferenceSpecifications { get; set; }

    [CliOption("--skip-model-validation")]
    public string? SkipModelValidation { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}