using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("globalaccelerator", "describe-cross-account-attachment")]
public record AwsGlobalacceleratorDescribeCrossAccountAttachmentOptions(
[property: CliOption("--attachment-arn")] string AttachmentArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}