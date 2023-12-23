using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("support", "add-attachments-to-set")]
public record AwsSupportAddAttachmentsToSetOptions(
[property: CommandSwitch("--attachments")] string[] Attachments
) : AwsOptions
{
    [CommandSwitch("--attachment-set-id")]
    public string? AttachmentSetId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}