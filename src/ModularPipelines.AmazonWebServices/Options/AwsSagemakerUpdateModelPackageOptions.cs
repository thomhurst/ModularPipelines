using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "update-model-package")]
public record AwsSagemakerUpdateModelPackageOptions(
[property: CommandSwitch("--model-package-arn")] string ModelPackageArn
) : AwsOptions
{
    [CommandSwitch("--model-approval-status")]
    public string? ModelApprovalStatus { get; set; }

    [CommandSwitch("--approval-description")]
    public string? ApprovalDescription { get; set; }

    [CommandSwitch("--customer-metadata-properties")]
    public IEnumerable<KeyValue>? CustomerMetadataProperties { get; set; }

    [CommandSwitch("--customer-metadata-properties-to-remove")]
    public string[]? CustomerMetadataPropertiesToRemove { get; set; }

    [CommandSwitch("--additional-inference-specifications-to-add")]
    public string[]? AdditionalInferenceSpecificationsToAdd { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}