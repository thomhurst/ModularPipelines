using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("globalaccelerator", "delete-cross-account-attachment")]
public record AwsGlobalacceleratorDeleteCrossAccountAttachmentOptions(
[property: CommandSwitch("--attachment-arn")] string AttachmentArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}