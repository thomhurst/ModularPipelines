using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "update-model-package")]
public record AwsSagemakerUpdateModelPackageOptions(
[property: CliOption("--model-package-arn")] string ModelPackageArn
) : AwsOptions
{
    [CliOption("--model-approval-status")]
    public string? ModelApprovalStatus { get; set; }

    [CliOption("--approval-description")]
    public string? ApprovalDescription { get; set; }

    [CliOption("--customer-metadata-properties")]
    public IEnumerable<KeyValue>? CustomerMetadataProperties { get; set; }

    [CliOption("--customer-metadata-properties-to-remove")]
    public string[]? CustomerMetadataPropertiesToRemove { get; set; }

    [CliOption("--additional-inference-specifications-to-add")]
    public string[]? AdditionalInferenceSpecificationsToAdd { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}