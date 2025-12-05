using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("support", "create-case")]
public record AwsSupportCreateCaseOptions(
[property: CliOption("--subject")] string Subject,
[property: CliOption("--communication-body")] string CommunicationBody
) : AwsOptions
{
    [CliOption("--service-code")]
    public string? ServiceCode { get; set; }

    [CliOption("--severity-code")]
    public string? SeverityCode { get; set; }

    [CliOption("--category-code")]
    public string? CategoryCode { get; set; }

    [CliOption("--cc-email-addresses")]
    public string[]? CcEmailAddresses { get; set; }

    [CliOption("--language")]
    public string? Language { get; set; }

    [CliOption("--issue-type")]
    public string? IssueType { get; set; }

    [CliOption("--attachment-set-id")]
    public string? AttachmentSetId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}