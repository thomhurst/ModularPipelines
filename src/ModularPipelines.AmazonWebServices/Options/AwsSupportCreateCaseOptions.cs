using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("support", "create-case")]
public record AwsSupportCreateCaseOptions(
[property: CommandSwitch("--subject")] string Subject,
[property: CommandSwitch("--communication-body")] string CommunicationBody
) : AwsOptions
{
    [CommandSwitch("--service-code")]
    public string? ServiceCode { get; set; }

    [CommandSwitch("--severity-code")]
    public string? SeverityCode { get; set; }

    [CommandSwitch("--category-code")]
    public string? CategoryCode { get; set; }

    [CommandSwitch("--cc-email-addresses")]
    public string[]? CcEmailAddresses { get; set; }

    [CommandSwitch("--language")]
    public string? Language { get; set; }

    [CommandSwitch("--issue-type")]
    public string? IssueType { get; set; }

    [CommandSwitch("--attachment-set-id")]
    public string? AttachmentSetId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}