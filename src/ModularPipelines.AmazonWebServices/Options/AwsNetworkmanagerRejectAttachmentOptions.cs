using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkmanager", "reject-attachment")]
public record AwsNetworkmanagerRejectAttachmentOptions(
[property: CommandSwitch("--attachment-id")] string AttachmentId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}