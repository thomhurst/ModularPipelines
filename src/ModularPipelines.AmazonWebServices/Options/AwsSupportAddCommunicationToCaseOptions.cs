using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("support", "add-communication-to-case")]
public record AwsSupportAddCommunicationToCaseOptions(
[property: CommandSwitch("--communication-body")] string CommunicationBody
) : AwsOptions
{
    [CommandSwitch("--case-id")]
    public string? CaseId { get; set; }

    [CommandSwitch("--cc-email-addresses")]
    public string[]? CcEmailAddresses { get; set; }

    [CommandSwitch("--attachment-set-id")]
    public string? AttachmentSetId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}