using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-email", "send-email")]
public record AwsPinpointEmailSendEmailOptions(
[property: CommandSwitch("--destination")] string Destination,
[property: CommandSwitch("--content")] string Content
) : AwsOptions
{
    [CommandSwitch("--from-email-address")]
    public string? FromEmailAddress { get; set; }

    [CommandSwitch("--reply-to-addresses")]
    public string[]? ReplyToAddresses { get; set; }

    [CommandSwitch("--feedback-forwarding-email-address")]
    public string? FeedbackForwardingEmailAddress { get; set; }

    [CommandSwitch("--email-tags")]
    public string[]? EmailTags { get; set; }

    [CommandSwitch("--configuration-set-name")]
    public string? ConfigurationSetName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}