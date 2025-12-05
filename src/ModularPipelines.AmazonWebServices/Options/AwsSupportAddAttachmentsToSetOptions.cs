using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("support", "add-attachments-to-set")]
public record AwsSupportAddAttachmentsToSetOptions(
[property: CliOption("--attachments")] string[] Attachments
) : AwsOptions
{
    [CliOption("--attachment-set-id")]
    public string? AttachmentSetId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}