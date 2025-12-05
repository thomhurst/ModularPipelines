using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("support", "add-communication-to-case")]
public record AwsSupportAddCommunicationToCaseOptions(
[property: CliOption("--communication-body")] string CommunicationBody
) : AwsOptions
{
    [CliOption("--case-id")]
    public string? CaseId { get; set; }

    [CliOption("--cc-email-addresses")]
    public string[]? CcEmailAddresses { get; set; }

    [CliOption("--attachment-set-id")]
    public string? AttachmentSetId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}